using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using HTTPServer.Models;

namespace HTTPServer.MyORM;

public class SteamAccounts : SteamAccountRepository
{
    private static readonly Func<SteamAccount> CreateInstance;
    private readonly string connectionString;
    private readonly string _Table = "Accounts";
    private static readonly PropertyInfo[] properties = typeof(SteamAccount).GetProperties();
    private static readonly IEnumerable<PropertyInfo> withoutId = properties.Where(p => p.Name != "Id");
    private static readonly IEnumerable<string> _fields = withoutId.Select(x => x.Name);

    static SteamAccounts()
    {
        var ctor = typeof(SteamAccount).GetConstructor(Type.EmptyTypes);
        var newExpr = Expression.New(ctor);

        var lambda = Expression.Lambda(typeof(Func<SteamAccount>), newExpr);
        CreateInstance = (Func<SteamAccount>)lambda.Compile();
    }
    
    public SteamAccounts(string connection) => connectionString = connection;
    
    public void AddAccount(SteamAccount account)
    {
        var values = withoutId.Select(x => $"'{x.GetValue(account)}'");
        var connection = new SqlConnection(connectionString);
        var cursor = connection.CreateCommand();
        cursor.CommandText = $"INSERT INTO {_Table} ({string.Join(',', _fields)}) VALUES ({string.Join(',', values)})";
        
        OpenAndExecute(connection, () => cursor.ExecuteNonQuery());
    }

    public void RemoveAccount(SteamAccount account)
    {
        var where = GetConstraint(account);
        var connection = new SqlConnection(connectionString);
        var cursor = connection.CreateCommand();
        cursor.CommandText = $"DELETE FROM {_Table} WHERE {where}";
        OpenAndExecute(connection, () => cursor.ExecuteNonQuery());
    }

    public void UpdateAccount(SteamAccount account, SteamAccount newAccount)
    {
        var zipped = _fields.Zip(withoutId.Select(x => $"'{x.GetValue(newAccount)}'"))
            .Select(x => $"{x.First}={x.Second}");
        var set = string.Join(',', zipped);
        
        var where = GetConstraint(account);
        var connection = new SqlConnection(connectionString);
        var cursor = connection.CreateCommand();
        cursor.CommandText = $"UPDATE {_Table} SET {set} WHERE {where}";
        
        OpenAndExecute(connection, () => cursor.ExecuteNonQuery());
    }

    public IEnumerable<SteamAccount> Query(ISqlSpecification specification)
    {
        var connection = new SqlConnection(connectionString);
        var cursor = connection.CreateCommand();
        cursor.CommandText = $"SELECT * FROM {_Table} {specification.ToSqlClauses()}";
        var list = new LinkedList<SteamAccount>();
        
        OpenAndExecute(connection, () =>
        {
            var reader = cursor.ExecuteReader();
            while (reader.Read())
            {
                var obj = CreateInstance();
                foreach (var property in properties)
                    property.SetValue(obj, reader[property.Name]);
                list.AddLast(obj);
            }
        });

        return list;
    }

    private string GetConstraint(SteamAccount model)
    {
        var zipped = _fields.Zip(withoutId.Select(x => $"'{x.GetValue(model)}'"))
            .Select(x => $"{x.First}={x.Second}");

        return string.Join(" AND ", zipped);
    }
    
    private void OpenAndExecute(SqlConnection connection, Action action)
    {
        using (connection)
        {
            connection.Open();
            action();
        }
    }
}