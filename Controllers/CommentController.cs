using System.Xml.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

public class CommentController : Controller
{
    // DB接続用
    private readonly string _connectionString = "Data Source=lab.db";

    /// <summary>
    /// XSS実験画面
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index()
    {
        var model = new CommentLabResult
        {
            Comments = GetComments()
        };
        return View(new CommentLabResult());
    }

    /// <summary>
    /// XSS実行
    /// </summary>
    /// <param name="commentText"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Index(string commentText)
    {
        if (!string.IsNullOrWhiteSpace(commentText))
        {
            SaveComment(commentText);
        }

        var model = new CommentLabResult
        {
            CommentText = commentText,
            Comments = GetComments()
        };

        return View(model);
    }

    #region 内部メソッド

    private List<CommentItem> GetComments()
    {
        var comments = new List<CommentItem>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id, Text, CreateAt
            FROM Comments
            ORDER BY Id DESC
        ";

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            comments.Add(new CommentItem
            {
                Id = reader.GetInt32(0),
                Text = reader.GetString(1),
                CreateAt = reader.GetString(2)
            });
        }

        return comments;
    }

    private void SaveComment(string text)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Comments (Text, CreateAt)
            VALUES (@text, @createAt)
        ";

        command.Parameters.AddWithValue("@text", text);
        command.Parameters.AddWithValue("@createAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        command.ExecuteNonQuery();
    }

    #endregion
}