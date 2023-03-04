<<<<<<< HEAD
using DogAPI.Models;
=======
>>>>>>> 1c3de640dea512340f26cc236ec7141f7e0ae131
using Microsoft.AspNetCore.Mvc;
using DogAPI.Services;

namespace DogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BreedsController : Controller
    {
        [HttpGet]
<<<<<<< HEAD
        public IActionResult Breeds([FromQuery] BreedsFilter? filter, [FromQuery] int page = 1, [FromQuery] int limit = 16)
=======
        public IActionResult Breeds([FromQuery] int page, [FromQuery] int limit = 16)
>>>>>>> 1c3de640dea512340f26cc236ec7141f7e0ae131
        {
            if (page < 0)
                page = 1;
            if (limit < 0)
                limit = 16;
<<<<<<< HEAD

            var info = filter != null & filter!.IsFilter ? 
                BreedsRepository.GetBreeds(page, limit, filter!) : BreedsRepository.GetBreeds(page, limit) ;
            return Json(new {totalInStorage=info.Item1, breeds=info.Item2} );
        }

=======
            
            var breeds = BreedsRepository.AllBreedsShortCut.Skip((page-1) * limit).Take(limit);
            
            return Json(breeds);
        }

        /*[HttpGet]
        public IActionResult Breeds() => Json(BreedsRepository.AllBreedsShortCut);*/

        [HttpGet("Total")]
        public IActionResult GetTotalBreeds() => Json(new { total = BreedsRepository.AllBreedsShortCut.Count });

>>>>>>> 1c3de640dea512340f26cc236ec7141f7e0ae131
        [HttpGet("{id:int:min(1)}")]
        public IActionResult GetBreedById([FromRoute] int id)
        {
            var breed = BreedsRepository.AllBreeds.Find(x => x.id == id);
            return breed != null ? Json(breed) : Json(new { error = "Not found" });
        }
    }
}