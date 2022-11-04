using HTTPServer.Models;
namespace HTTPServer.MyORM;

public interface SteamAccountRepository
{
    void AddAccount(SteamAccount account);
    void RemoveAccount(SteamAccount account);
    void UpdateAccount(SteamAccount old, SteamAccount newAccount);
 
    IEnumerable<SteamAccount> Query(ISqlSpecification specification);
}