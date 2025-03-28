using Microsoft.AspNetCore.Mvc;

namespace Lab1_4.Controllers;

public class AccountController : Controller
{
    public IActionResult ToAccount()
    {
        return View("AccountMainPage");
    }
}