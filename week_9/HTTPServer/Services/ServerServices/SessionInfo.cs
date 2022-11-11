namespace HTTPServer.Services.ServerServices;

public record class SessionInfo
{
    public bool IsAuthorized { get; init; }
    
    public int AccountId { get; init; }
}