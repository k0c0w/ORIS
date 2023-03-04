using DogAPI.Dtoes;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace DogAPI.Services;

public class EmailService : IEmailService
{
    private const string MyEmail = "turkburksurk@gmail.com";
    private const string MyEmailPassword = "snrkmyvdioljgkuj";
    private const string SmtpHost = "smtp.gmail.com";
    private const string Body = "Thank you!";

    private readonly EmailConfig _emailConfig;

    public EmailService(IOptions<EmailConfig> configure) => _emailConfig = configure.Value;
    
    public async Task<object> SaveAndNotifyAsync(UserReviewDto review)
    {
        
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

    public async Task SendEmailAsync(string addressee, string body, string subject = "")
    {
        using var smtpClient = new SmtpClient(_emailConfig.MailServerAddress)
        {
            Port = _emailConfig.MailServerPort,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_emailConfig.FromEmail, _emailConfig.EmailPassword),
            EnableSsl = _emailConfig.EnableSsl,
        };
        await smtpClient.SendMailAsync(new MailMessage(_emailConfig.FromEmail, addressee, subject, body));
    }
}