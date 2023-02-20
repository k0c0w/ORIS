using Microsoft.AspNetCore.Mvc;
using DogAPI.Services;

namespace DogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BreedsController : Controller
    {
        [HttpGet]
        public IActionResult Breeds()
        {
            var breeds = BreedsRepository.AllBreedsShortCut;

            return Json(breeds);
        }

        [HttpGet("{guid}")]
        public IActionResult GetBreedById([FromRoute] int guid)
        {
            var breed = BreedsRepository.AllBreeds.Find(x => x.id == guid);
            return breed != null ? Json(breed) : Json(new { error = "Not found" });
        }
    }
}