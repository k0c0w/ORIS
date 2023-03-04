using System.Text.Json;
using DogAPI.Models;

namespace DogAPI.Services
{
    public static class BreedsRepository
    {

        static BreedsRepository()
        {
            using var breedsReader = new StreamReader(@".\Models\Breeds\breeds.json");
            var breeds = breedsReader.ReadToEnd();
            AllBreeds = JsonSerializer.Deserialize<List<Breed>>(breeds);
            using var breedsShortReader = new StreamReader(@".\Models\Breeds\breeds_short.json");
            var breedsShort = breedsShortReader.ReadToEnd();
            AllBreedsShortCut = JsonSerializer.Deserialize<List<BreedShortcut>>(breedsShort);
        }

        public static readonly List<Breed> AllBreeds;

        private static readonly List<BreedShortcut> AllBreedsShortCut;
        
        public static (int, IEnumerable<BreedShortcut>) GetBreeds(int page, int limit)
            => (AllBreedsShortCut.Count, AllBreedsShortCut.Skip((page - 1) * limit).Take(limit));

        public static (int, IEnumerable<BreedShortcut>) GetBreeds(int page, int limit, BreedsFilter filter)
        {
            filter.Normalize();

            var filtered = AllBreeds
                .Where(breed =>
                {
                    var breedFits = false;
                    if (filter.IsLifespan)
                        breedFits = filter.min_lifespan <= breed.life_span && breed.life_span <= filter.max_lifespan;

                    if (filter.IsHeight)
                        breedFits &= (filter.min_height <= breed.min_height &&
                                      breed.max_height <= filter.max_height);

                    if (filter.IsWeight)
                        breedFits &= (filter.min_weight <= breed.min_weight &&
                                      breed.max_weight <= filter.max_weight);
                    if (filter.IsBreedGroup)
                        breedFits &= filter.breed_group == breed.breed_group;
                    return breedFits;
                })
                .ToArray();
            
            return (filtered.Length, 
                filtered.Skip((page - 1) * limit)
                                             .Take(limit)
                                             .Select(breed => new BreedShortcut(
                                                 breed.id, breed.name, breed.image_url, breed.breed_group)));
        }
    }
}
