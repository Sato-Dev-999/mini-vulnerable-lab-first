using Microsoft.Data.Sqlite;

public class DatabaseInitializer
{
    private readonly string _connectionString = "Data Source=lab.db";

    public void Initialize()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var createCommand = connection.CreateCommand();
        createCommand.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL,
                Password TEXT NOT NULL
                );
                
                DELETE FROM Users;
                
                INSERT INTO Users (Username, Password) VALUES
                ('admin', 'password123'),
                ('satoshi', 'test123'),
                ('guest', 'guest');
            ";
            createCommand.ExecuteNonQuery();
    }
}