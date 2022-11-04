using HTTPServer.Models;
using System.Data.SqlClient;
using MyORM;

namespace HTTPServer.Services
{
    public static class DBProvider
    {
        static DatabaseProvider db = new DatabaseProvider( @"Server=localhost;Database=test;Trusted_Connection=True;");

        public static SteamAccount? GetSteamAccount(int id)
        {
            return db.Select<SteamAccount>(new WhereModel<SteamAccount>(new SteamAccount() { Id = id }))
                .FirstOrDefault();
        }

        public static IEnumerable<SteamAccount> GetSteamAccounts()
        {
            return db.Select<SteamAccount>();
        }

        public static Task<int> WriteToDatabaseAsync(SteamAccount user)
        {
            var t =  new Task<int>(() => db.Insert(user));
            t.Start();
            return t;
        }
    }
}
