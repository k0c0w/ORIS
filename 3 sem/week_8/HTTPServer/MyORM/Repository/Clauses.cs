namespace HTTPServer.MyORM;

public class EmptyClauses : ISqlSpecification
{
    public string ToSqlClauses() => "";
}

public class SelectIdClauses : ISqlSpecification
{
    private readonly int id;

    public SelectIdClauses(int id) => this.id = id;


    public string ToSqlClauses() => $"WHERE Id = {id}";
}