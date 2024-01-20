namespace AspNetCoreExample;

public class DatabasesOptions
{
    public DatabaseConnection User { get; set; } = new();

    public DatabaseConnection Movie { get; set; } = new();
}

public class DatabaseConnection
{
    public string Host { get; set; } = "";

    public int Port { get; set; }

    public string Database { get; set; } = "";
}