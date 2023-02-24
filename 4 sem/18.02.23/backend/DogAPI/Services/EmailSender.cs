using DogAPI.Dtoes;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace DogAPI.Services;

public class EmailSender
{
    private const string MyEmail = "turkburksurk@gmail.com";
    private const string MyEmailPassword = "snrkmyvdioljgkuj";
    private const string SmtpHost = "smtp.gmail.com";
    private const string Body = "Thank you!";
    
    
    public async Task<object> SaveAndNotifyAsync(UserReviewDto review)
    {
        if (!Regex.IsMatch(review.email, @"[a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\.[a-zA-Z0-9_-]+", RegexOptions.IgnoreCase,
                TimeSpan.FromSeconds(10)))
            return new { Error = "Incorrect email." };
        try
        {
            using var smtpClient = new SmtpClient(SmtpHost)
            {
                Port = 587,
                Credentials = new NetworkCredential(MyEmail, MyEmailPassword),
                EnableSsl = true,
            };


            await smtpClient.SendMailAsync(new MailMessage(MyEmail, review.email, "Thanks!", Body));
            await smtpClient.SendMailAsync(
                new MailMessage(MyEmail, MyEmail, $"{review.name}`s review", review.review));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new {Error="Error occured. Email was not saved."};
        }

        return new {Success="Email sent"};
    }
}