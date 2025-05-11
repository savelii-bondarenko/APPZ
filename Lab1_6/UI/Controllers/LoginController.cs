using Lab1_6.BusinessLogic.Services;
using Lab1_6.Models;
using Lab1_6.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lab1_6.UI.Controllers;

public class LoginController(UserService userService, IPasswordHasher<User> passwordHasher)
    : Controller
{
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

        var user = userService.GetByEmail(model.Email);
        if (user != null)
        {
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
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