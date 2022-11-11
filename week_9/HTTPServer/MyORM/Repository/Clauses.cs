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

public record class SelectEmailAndPasswordClauses : ISqlSpecification
{
    public string Email { get; init; }
    public string Password { get; init; }

    public string ToSqlClauses() => $"WHERE Login = '{Email}' AND Password = '{Password}'";
}