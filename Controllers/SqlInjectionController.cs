using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

/// <summary>
/// SQLインジェクション
/// </summary>
public class SqlInjectionController : Controller
{
    // DB接続用
    private readonly string _connectionString = "Data Source=lab.db";

    /// <summary>
    /// トップ画面
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index()
    {
        return View(new SqlInjectionResult());
    }

    /// <summary>
    /// SQLインジェクション実行
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Index(string username, string password)
    {
        var model = new SqlInjectionResult
        {
            Username = username,
            Password = password
        };

        try
        {
            // 脆弱版の実行結果
            model.VulnerableLoginSuccess = ExecuteVulnerableLogin(username, password, model);
            // 安全版の実行結果
            model.SafeLoginSuccess = ExecuteSafeLogin(username, password, model);
        }
        catch (Exception ex)
        {
            model.ErrorMessage = ex.Message;
        }

        return View(model);
    }

    #region 内部メソッド

    /// <summary>
    /// 脆弱版の実行処理
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    private bool ExecuteVulnerableLogin(string username, string password, SqlInjectionResult model)
    {
        // SQL文字列そのものを連結
        string sql = $"SELECT COUNT(*) FROM Users WHERE Username = '{username}' AND Password = '{password}'";

        // 脆弱版SQL
        model.VulnerableSql = sql;

        /*
        SQLiteへの接続オブジェクト生成
        　using var ← 処理終了時に自動Dispose
        */
        using var connection = new SqliteConnection(_connectionString);

        // 接続開始
        connection.Open();

        // SQL実行オブジェクト
        using var command = connection.CreateCommand();
        // SQLをセット
        command.CommandText = sql;

        // SQL実行
        // ExwcuteScalar ← １件だけ取得
        // SQLiteの戻り値はobject型 → int型に変換
        var count = Convert.ToInt32(command.ExecuteScalar());

        return count > 0;
    }

    /// <summary>
    /// 安全版の実行処理
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    private bool ExecuteSafeLogin(string username, string password, SqlInjectionResult model)
    {
        // SQLの雛形を作成
        string sql = "SELECT COUNT(*) FROM Users WHERE Username = @username AND Password = @password";

        // 安全版SQL
        model.SafeSql = sql;

        // SQLiteへの接続オブジェクトを生成
        using var connection = new SqliteConnection(_connectionString);
        // DB接続
        connection.Open();

        // SQL実行オブジェクト生成
        using var command = connection.CreateCommand();
        // SQLをセット
        command.CommandText = sql;

        // SQLのパラメーターに値を渡す
        command.Parameters.AddWithValue("@username", username);
        command.Parameters.AddWithValue("@password", password);

        // SQL実行
        // ExwcuteScalar ← １件だけ取得
        // SQLiteの戻り値はobject型 → int型に変換
        var count = Convert.ToInt32(command.ExecuteScalar());

        return count > 0;
    }

    #endregion
}