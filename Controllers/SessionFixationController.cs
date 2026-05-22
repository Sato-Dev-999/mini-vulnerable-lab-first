using Microsoft.AspNetCore.Mvc;

public class SessionFixationController : Controller
{
    /// <summary>
    /// Dictionary : 辞書型のデータ構造(key, value)
    /// </summary>
    private static readonly Dictionary<string, string> Sessions = new();

    /// <summary>
    /// ログイン画面表示
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Login(string? sessionId)
    {
        if (!string.IsNullOrEmpty(sessionId))
        {
            Response.Cookies.Append("LabSessionId", sessionId);
            ViewBag.Message = $"外部から渡されたセッションIDをセット： {sessionId}";
        }

        return View();
    }

    /// <summary>
    /// ログイン処理:脆弱版
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    // [HttpPost]
    // public IActionResult Login(string username, string password)
    // {
    //     var sessionId = Request.Cookies["LabSessionId"];

    //     if (string.IsNullOrEmpty(sessionId))
    //     {
    //         // セッションIDにGUIDをセット
    //         sessionId = Guid.NewGuid().ToString();
    //         Response.Cookies.Append("LabSessionId", sessionId);
    //     }

    //     if (username == "satoshi" && password == "password")
    //     {
    //         Sessions[sessionId] = username;
    //         return RedirectToAction("MyPage");
    //     }

    //     ViewBag.Error = "ログイン失敗";
    //     return View();
    // }

    /// <summary>
    /// ログイン処理:安全版
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Login (string username, string password)
    {
        if (username == "satoshi" && password == "password")
        {
            var oldSessionId = Request.Cookies["LabSessionId"];

            if (!string.IsNullOrEmpty(oldSessionId))
            {
                Sessions.Remove(oldSessionId);
            }

            var newSessionId = Guid.NewGuid().ToString();

            Response.Cookies.Append("LabSessionId", newSessionId);
            Sessions[newSessionId] = username;

            return RedirectToAction("MyPage");
        }

        ViewBag.Error = "ログイン失敗";
        return View();
    }

    /// <summary>
    /// ログイン後処理
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult MyPage()
    {
        var sessionId = Request.Cookies["LabSessionId"];

        if (string.IsNullOrEmpty(sessionId) || !Sessions.ContainsKey(sessionId))
        {
            return Unauthorized();
        }

        var username = Sessions[sessionId];

        return Content($"ログイン中ユーザー: {username}\nSessionId: {sessionId}");
    }
}