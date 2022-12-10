using System.Drawing;

namespace TCPChat;

public record class User
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public Color Color { get; init; }
}