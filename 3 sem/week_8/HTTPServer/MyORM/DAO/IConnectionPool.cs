using System.Data;

namespace HTTPServer.MyORM;

public interface IConnectionPool
{
    IDbConnection GetConnection();
    void ReturnConnection(IDbConnection connection);
}