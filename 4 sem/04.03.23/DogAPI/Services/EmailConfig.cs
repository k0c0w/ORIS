namespace DogAPI.Services;

public class EmailConfig
{
    public string FromEmail { get; init; }
    
    public string MailServerAddress { get; init; }
    
    public int MailServerPort { get; init; }

    public string EmailPassword { get; init; }
    
    
    public bool EnableSsl { get; init; }
}