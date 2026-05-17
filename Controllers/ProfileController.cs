using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;

public class ProfileController : Controller
{
    private static string _displayName = "satoshi";

    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.DisplayName = _displayName;
        return View();
    }

    /// <summary>
    /// 脆弱版：GET（安全版にするならメソッドごと削除）
    /// </summary>
    /// <param name="displayName"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult ChangeNameByGet(string displayName)
    {
        _displayName = displayName;
        return RedirectToAction("Index");
    }

    /// <summary>
    /// 脆弱版：POST
    /// </summary>
    /// <param name="displayName"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult ChangeNameByPost(string displayName)
    {
        _displayName = displayName;
        return RedirectToAction("Index");
    }

    /// <summary>
    /// 安全版：POST
    /// </summary>
    /// <param name="displayName"></param>
    /// <returns></returns>
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public IActionResult ChangeName(string displayName)
    // {
    //     _displayName = displayName;
    //     return RedirectToAction("Index");
    // }
}
