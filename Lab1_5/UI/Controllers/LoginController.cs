using Lab1_5.BusinessLogic.Services;
using Lab1_5.Models;
using Lab1_5.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lab1_5.UI.Controllers;

public class LoginController : Controller
{
    private readonly UserService _userService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public LoginController(UserService userService, IPasswordHasher<User> passwordHasher)
    {
        _userService = userService;
        _passwordHasher = passwordHasher;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new LoginModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(LoginModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = _userService.GetByEmail(model.Email);
        if (user != null)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
            if (result == PasswordVerificationResult.Success)
            {
                HttpContext.Session.SetString("UserEmail", user.Email);
                TempData["Message"] = "Login successful!";
                return RedirectToAction("Index", "Account");
            }
        }

        ModelState.AddModelError("", "Invalid email or password.");
        return View(model);
    }
}