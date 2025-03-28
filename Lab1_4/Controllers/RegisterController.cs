using System.Security.Cryptography;
using Lab1_4.Data;
using Lab1_4.Models;
using Lab1_4.Models.Entity;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;

namespace Lab1_4.Controllers;

public class RegisterController : Controller
{
    private readonly AppDbContext _context;

    public RegisterController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Register()
    {
        return View(new RegisterModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (_context.Users.Any(x => x.Email == model.Email))
        {
            ModelState.AddModelError("Email", "Email is already taken.");
            return View(model);
        }

        var hashedPassword = HashPassword(model.Password);

        var user = new User
        {
            Email = model.Email,
            Password = hashedPassword,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return RedirectToAction("ToAccount", "Account");
    }

    private string HashPassword(string password)
    {
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return $"{Convert.ToBase64String(salt)}.{hashed}";
    }
}