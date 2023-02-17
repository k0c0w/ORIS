using System.ComponentModel.DataAnnotations.Schema;

namespace HTTPServer.Models
{
    [Table("Accounts")]
    public record class SteamAccount
    {
        public int? Id { get; init; } = null;
        public string? Login { get; init; }
        public string? Password { get; init; }
        
        public override string ToString()
        {
            return $"Steam Account: (Login: {Login}, Password: {Password})";
        }
    }
}
