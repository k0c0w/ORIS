using System.Net.Mail;


namespace DogAPI.Services;

public interface IEmailService
{
    public Task SendEmailAsync(string addressee, string message, string subject = "");
}