using HTTPServer.Models;
using System.Data.SqlClient;
using HTTPServer.MyORM;

namespace HTTPServer.Services
{
    public static class DBProvider
    {
        private static SteamAccounts repo = new SteamAccounts(@"Server=localhost;Database=test;Trusted_Connection=True;");
        public static SteamAccount? GetSteamAccount(int id)
        {
            return repo.Query(new SelectIdClauses(id)).FirstOrDefault();
        }

        public static SteamAccount? GetSteamAccount(SteamAccount searchFor)
        {
            return repo.Query(new SelectEmailAndPasswordClauses()
                { Email = searchFor.Login, Password = searchFor.Password })
                .FirstOrDefault();
        }

        public static IEnumerable<SteamAccount> GetSteamAccounts()
        {
            return repo.Query(new EmptyClauses());
        }

        public static Task<int> WriteToDatabaseAsync(SteamAccount user)
        {
            var t = new Task<int>(() =>
            {
                repo.AddAccount(user);
                return 1;
            });
            t.Start();
            return t;
        }
    }
}
