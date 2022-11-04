using System.Data;
using System.Data.SqlClient;

namespace ORM;

public class ConnectionPool : IConnectionPool
{
    private static Queue<IDbConnection> connections = new Queue<IDbConnection>();

    static ConnectionPool()
    {
        SetConnections(new []{@"Server=localhost;Database=test;Trusted_Connection=True;"});
    }
    
    private ConnectionPool() { }

    public static ConnectionPool GetConnectionPool() => new ConnectionPool();

    public static void SetConnections(IEnumerable<string> connectionStrings)
    {
        connections.Clear();
        foreach (var elem in connectionStrings)
            connections.Enqueue(new SqlConnection(elem));
    }
    
    public IDbConnection GetConnection()
    {
        return connections.Dequeue();
    }

    public void ReturnConnection(IDbConnection connection)
    {
        connections.Enqueue(connection);
    }
}