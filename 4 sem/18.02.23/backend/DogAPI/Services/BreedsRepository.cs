using DogAPI.Models;

namespace DogAPI.Services
{
    public static class BreedsRepository
    {

        public static List<Breed> AllBreeds = new List<Breed>() 
        {
            new Breed(1, "Stubborn, Curious, Playful, Adventurous, Active, Fun-loving", "Germany, France", "Affenpinscher", 6, 13, 6, 13, new TimeSpan(150000000), "Small rodent hunting, lapdog", "Toy", "https://cdn2.thedogapi.com/images/BJa4kxc4X_1280.jpg"),
        };

        public static List<BreedShortcut> AllBreedsShortCut = new List<BreedShortcut>()
        {
            new BreedShortcut(1, "Affenpinscher", "https://cdn2.thedogapi.com/images/BJa4kxc4X_1280.jpg", "Toy")
        };
    }
}
