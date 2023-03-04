using DogAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DogAPI.Services;

namespace DogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BreedsController : Controller
    {
        [HttpGet]
        public IActionResult Breeds([FromQuery] BreedsFilter? filter, [FromQuery] int page = 1, [FromQuery] int limit = 16)
        {
            if (page < 0)
                page = 1;
            if (limit < 0)
                limit = 16;

            var info = filter != null & filter!.IsFilter ? 
                BreedsRepository.GetBreeds(page, limit, filter!) : BreedsRepository.GetBreeds(page, limit) ;
            return Json(new {totalInStorage=info.Item1, breeds=info.Item2} );
        }

        [HttpGet("{id:int:min(1)}")]
        public IActionResult GetBreedById([FromRoute] int id)
        {
            var breed = BreedsRepository.AllBreeds.Find(x => x.id == id);
            return breed != null ? Json(breed) : Json(new { error = "Not found" });
        }
    }
}