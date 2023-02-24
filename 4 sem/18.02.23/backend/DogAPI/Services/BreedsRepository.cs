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

        public static readonly List<BreedShortcut> AllBreedsShortCut;
    }
}
