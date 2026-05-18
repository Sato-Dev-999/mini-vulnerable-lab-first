using Microsoft.AspNetCore.Mvc;

public class OpenRedirectController : Controller
{
    /// <summary>
    /// ログイン画面
    /// </summary>
    /// <param name="returnUrl"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Login(string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    /// <summary>
    /// ログイン処理：脆弱版
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="returnUrl"></param>
    /// <returns></returns>
    // [HttpPost]
    // public IActionResult Login(string userName, string password, string? returnUrl)
    // {
    //     // ログイン情報が一致した場合
    //     if (userName == "satoshi" && password == "password")
    //     {
    //         // クエリパラメータでリターンURLが指定された場合はそのURLにリダイレクト
    //         if (!string.IsNullOrEmpty(returnUrl))
    //         {
    //             return Redirect(returnUrl);
    //         }

    //         // URL指定がない場合はサイト内のページにリダイレクト
    //         return RedirectToAction("SafePage");
    //     }

    //     // ログイン情報不一致
    //     ViewBag.Error = "ログイン失敗";
    //     ViewBag.ReturnUrl = returnUrl;

    //     return View();
    // }

    /// <summary>
    /// ログイン処理：安全版
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="returnUrl"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Login(string userName, string password, string? returnUrl)
    {
        // ログイン情報が一致した場合
        if (userName == "satoshi" && password == "password")
        {
            // サイト内のURLのみリダイレクト許可
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            // URL指定がない場合はサイト内のページにリダイレクト
            return RedirectToAction("SafePage");
        }

        // ログイン情報不一致
        ViewBag.Error = "ログイン失敗";
        ViewBag.ReturnUrl = returnUrl;

        return View();
    }

    /// <summary>
    /// 安全な画面表示
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult SafePage()
    {
        return Content("ログイン後の安全なページ");
    }
}