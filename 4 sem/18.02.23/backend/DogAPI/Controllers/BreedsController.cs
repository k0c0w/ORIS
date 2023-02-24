using Microsoft.AspNetCore.Mvc;
using DogAPI.Services;

namespace DogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BreedsController : Controller
    {
        [HttpGet]
        public IActionResult Breeds([FromQuery] int page, [FromQuery] int limit = 16)
        {
            if (page < 0)
                page = 1;
            if (limit < 0)
                limit = 16;
            
            var breeds = BreedsRepository.AllBreedsShortCut.Skip((page-1) * limit).Take(limit);
            
            return Json(breeds);
        }

        /*[HttpGet]
        public IActionResult Breeds() => Json(BreedsRepository.AllBreedsShortCut);*/

        [HttpGet("Total")]
        public IActionResult GetTotalBreeds() => Json(new { total = BreedsRepository.AllBreedsShortCut.Count });

        [HttpGet("{id:int:min(1)}")]
        public IActionResult GetBreedById([FromRoute] int id)
        {
            var breed = BreedsRepository.AllBreeds.Find(x => x.id == id);
            return breed != null ? Json(breed) : Json(new { error = "Not found" });
        }
    }
}