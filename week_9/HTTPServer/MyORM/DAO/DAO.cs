using System.Data;

namespace HTTPServer.MyORM;

public abstract class DAO<TModel, Pk> : IDisposable
{
    private IDbConnection _connection;
    private ConnectionPool _connectionPool;

    public DAO()
    {
        _connectionPool = ConnectionPool.GetConnectionPool();
        _connection = _connectionPool.GetConnection();
    }

    public void returnConnectionInPool() {
        if(_connection != null)
            _connectionPool.ReturnConnection(_connection);
        _connection = null;
    }
    
    public IDbCommand GetPrepareStatement(string sql) 
    {
        IDbCommand ps = null;
        try 
        {
            _connection.Open();
            ps = _connection.CreateCommand();
            ps.CommandText = sql;
        } 
        catch 
        {
        }

        return ps;
    }

    public void CLosePrepareStatement(IDbCommand ps)
    {
        if(_connection != null)
            _connection.Close();
        if(ps != null)
            ps.Dispose();
    }
    
    public abstract IEnumerable<TModel> GetAll();
    public abstract TModel GetEntityById(Pk id);
    public abstract bool Update(Pk id, TModel newModel);
    public abstract bool Delete(Pk id);
    public abstract bool Create(TModel model);

    public void Dispose()
    {
        if(_connection != null)
            _connectionPool.ReturnConnection(_connection);
    }
}