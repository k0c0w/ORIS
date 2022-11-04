using HTTPServer.Models;
using System.Data.SqlClient;
using MyORM;

namespace HTTPServer.Services
{
    public static class DBProvider
    {
        private static AccountDAO accountDAO = new AccountDAO(new ConsoleLogger()); 
        public static SteamAccount? GetSteamAccount(int id)
        {
            return accountDAO.GetEntityById(id);
        }

        public static IEnumerable<SteamAccount> GetSteamAccounts()
        {
            return accountDAO.GetAll();
        }

        public static Task<int> WriteToDatabaseAsync(SteamAccount user)
        {
            var t =  new Task<int>(() => accountDAO.Create(user) ? 1 : 0);
            t.Start();
            return t;
        }
    }
}
