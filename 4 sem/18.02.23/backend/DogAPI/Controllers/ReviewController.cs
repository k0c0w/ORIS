using DogAPI.Dtoes;
using DogAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DogAPI.Controllers;

[Route("[controller]")]
public class ReviewController : Controller
{
    private EmailSender sender;
    
    public ReviewController()
    {
        sender = new EmailSender();
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Review([FromBody] UserReviewDto review)
    {
        var emailSavedDto = await sender.SaveAndNotifyAsync(review);
        return Json(new { Error = emailSavedDto });
    }
}