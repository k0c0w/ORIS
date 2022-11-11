using System.Linq.Expressions;
using System.Reflection;
using HTTPServer.Models;
using HTTPServer.MyORM;

namespace HTTPServer.Services;

public class AccountDAO : DAO<SteamAccount, int>
{
    private static readonly Func<SteamAccount> CreateInstance;
    private static readonly Type modelType = typeof(SteamAccount);
    private static readonly IEnumerable<PropertyInfo> propeties = modelType.GetProperties();
    private readonly ILogger logger;
    
    static AccountDAO()
    {
        var ctor = typeof(SteamAccount).GetConstructor(Type.EmptyTypes);
        var newExpr = Expression.New(ctor);

        var lambda = Expression.Lambda(typeof(Func<SteamAccount>), newExpr);
        CreateInstance = (Func<SteamAccount>)lambda.Compile();
    }

    public AccountDAO(ILogger logger) => this.logger = logger;
    
    public override IEnumerable<SteamAccount> GetAll()
    {
        var list = new LinkedList<SteamAccount>();
        var cursor = GetPrepareStatement("SELECT * FROM Accounts");
        try
        {
            var reader = cursor.ExecuteReader();
            while (reader.Read())
            {
                var obj = CreateInstance();
                foreach (var property in propeties)
                {
                    property.SetValue(obj, reader[property.Name]);
                }

                list.AddLast(obj);
            }
        }
        catch (Exception e)
        {
            logger.ReportError(e.ToString());
        }
        finally
        {
            CLosePrepareStatement(cursor);
        }

        return list;
    }

    public override SteamAccount GetEntityById(int id)
    {
        var obj = CreateInstance();
        var cursor = GetPrepareStatement($"SELECT * FROM Accounts WHERE Id = '{id}'");
        try
        {
            var reader = cursor.ExecuteReader();
            if(reader.Read())
                foreach (var property in propeties)
                {
                    property.SetValue(obj, reader[property.Name]);
                }
        }
        catch (Exception e)
        {
            logger.ReportError(e.ToString());
        }
        finally
        {
            CLosePrepareStatement(cursor);
        }

        return obj;
    }

    public override bool Update(int id, SteamAccount newModel)
    {
        var fields = propeties.Where(p => p.Name != "Id");
        var values = fields.Select(x => x.GetValue(newModel));
        var zipped = fields.Zip(values).Select(x => $"{x.First.Name}='{x.Second}'");
        return TryExecute($"UPDATE Accounts SET {string.Join(',', zipped)}");
    }

    public override bool Delete(int id)
        => TryExecute($"DELETE FROM Accounts WHERE Id = '{id}'");

    public override bool Create(SteamAccount model)
    {
        var fields = propeties.Where(p => p.Name != "Id");
        var values = fields.Select(x => $"'{x.GetValue(model)}'");
        var sql = $"INSERT INTO Accounts({string.Join(',', fields.Select(x => x.Name))})"
                  + $" VALUES({string.Join(',', values)})";
        return TryExecute(sql);
    }

    private bool TryExecute(string sql)
    {
        var cursor = GetPrepareStatement(sql);
        try
        {
            var affected = cursor.ExecuteNonQuery();
            return affected != 0;
        }
        catch (Exception e)
        {
            logger.ReportError(e.ToString());
            return false;
        }
        finally
        {
            CLosePrepareStatement(cursor);
        }
    }
}