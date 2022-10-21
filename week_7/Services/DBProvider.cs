using HTTPServer.Models;
using System.Data.SqlClient;

namespace HTTPServer.Services
{
    public static class DBProvider
    {
        static string connectionString = @"Data Source=(localdb)MSSQLLocalDB;Initial Catalog=AppDB;Integrated Security=True";

        static Func<string, List<SteamAccount>> getAccounts = (predicate) =>
        {
            var users = new List<SteamAccount>();

            string sqlExpression = $"SELECT * FROM [dbo].[Accounts] {predicate}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        users.Add(new SteamAccount()
                        {
                            Login = reader.GetString(0),
                            Password = reader.GetString(1)
                        });
                    }
                }

                reader.Close();
            }
            return users;
        };

        public static SteamAccount? GetSteamAccount(int id)
        {
            var users = getAccounts($"WHERE id = {id}");
            return users.Count == 1 ? users[0] : null;
        }

        public static List<SteamAccount> GetSteamAccounts()
        {
            return getAccounts("");
        }

        public static async Task<bool> WriteToDatabaseAsync(SteamAccount user)
        {
            return true;
            string sqlExpression = $"INSERT INTO [dbo].[Accounts](login, password) VALUES('{user.Login}', {user.Password})";
            int result = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                result = await command.ExecuteNonQueryAsync();
            }
            return result != 0;
        }
    }
}
