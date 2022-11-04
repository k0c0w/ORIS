using System.Data;

namespace ORM;

public interface IConnectionPool
{
    IDbConnection GetConnection();
    void ReturnConnection(IDbConnection connection);
}