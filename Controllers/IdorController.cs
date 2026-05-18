using Microsoft.AspNetCore.Mvc;

public class IdorController : Controller
{

    private static readonly List<LabOrder> Orders = new()
    {
        new LabOrder
        {
            Id = 101,
            UserId = 1,
            ItemName = "キーボード",
            Price = 12000,
            Address = "東京都大田区蒲田"
        },
        new LabOrder
        {
            Id = 102,
            UserId = 1,
            ItemName = "ノートPC",
            Price = 200000,
            Address = "東京都大田区蒲田"
        },
        new LabOrder
        {
            Id = 201,
            UserId = 2,
            ItemName = "冷蔵庫",
            Price = 80000,
            Address = "神奈川県横浜市金沢区"
        }
    };

    // 仮ログインユーザー
    private const int LoginUserId = 1;

    [HttpGet]
    public IActionResult OrderList()
    {
        var myOrders = Orders
            .Where(x => x.UserId == LoginUserId)
            .ToList();

        ViewBag.LoginUserId = LoginUserId;
        return  View(myOrders);
    }

    /// <summary>
    /// 脆弱版
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // [HttpGet]
    // public IActionResult OrderDetail(int id)
    // {
    //     var order = Orders.FirstOrDefault(x => x.Id == id);

    //     if (order == null)
    //     {
    //         return NotFound();
    //     }

    //     ViewBag.LoginUserId = LoginUserId;
    //     return View(order);
    // }

    /// <summary>
    /// 安全版
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult OrderDetail(int id)
    {
        var order = Orders.FirstOrDefault(x => x.Id == id && x.UserId == LoginUserId);

        if (order == null)
        {
            return NotFound();
        }

        ViewBag.LoginUserId = LoginUserId;
        return View(order);
    }
}