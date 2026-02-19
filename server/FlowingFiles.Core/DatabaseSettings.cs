namespace FlowingFiles.Core;

public enum DatabaseProvider
{
    PostgreSQL,
    SQLite
}

public class DatabaseSettings
{
    public DatabaseProvider Provider { get; private set; }
    public string? DatabaseUrl { get; private set; }
    public string DbPath { get; private set; } = string.Empty;

    public DatabaseSettings()
    {
        DatabaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

        if (!string.IsNullOrEmpty(DatabaseUrl))
        {
            Provider = DatabaseProvider.PostgreSQL;
            return;
        }

        Provider = DatabaseProvider.SQLite;

        var dbPath = Environment.GetEnvironmentVariable("DB_PATH");
        if (!string.IsNullOrEmpty(dbPath))
        {
            DbPath = dbPath;
            return;
        }

        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Combine(path, "FlowingFiles.db");
    }
}
