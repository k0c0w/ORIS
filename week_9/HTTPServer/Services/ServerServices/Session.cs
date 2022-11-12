namespace HTTPServer.Services.ServerServices;

public record class Session
{
    public int Id { get; init; }
    
    public int AccountId { get; init; }
    
    public string Email { get; init; }
    
    public DateTime CreateDateTime { get; init; }
}