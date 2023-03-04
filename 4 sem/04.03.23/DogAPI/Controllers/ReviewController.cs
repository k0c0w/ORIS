using DogAPI.Dtoes;
using DogAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace DogAPI.Controllers;

[Route("[controller]")]
public class ReviewController : Controller
{
    private readonly IEmailService _emailService;
    private readonly EmailConfig _config;

    public ReviewController(IEmailService emailService, IOptions<EmailConfig> config)
    {
        _emailService = emailService;
        _config = config.Value;
    }

    [HttpPost]
    public async Task<IActionResult> Review([FromBody] UserReviewDto review)
    {
        if (!Regex.IsMatch(review.email, @"[a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\.[a-zA-Z0-9_-]+", RegexOptions.IgnoreCase,
                TimeSpan.FromSeconds(10)))
            return Json(new { Error = "Incorrect email." });
        try
        { 
            await _emailService.SendEmailAsync(review.email, "Thank you");
            await _emailService.SendEmailAsync(_config.FromEmail, review.review, 
                $"review from {review.name}");
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Json(new { Error = "Error occured. Email was not sent." });
        }

        return Json(new { Success = "Email sent" });
    }
}