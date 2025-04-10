using System.Security.Cryptography;
using System.Text;
using Lab1_5.DataAccess;
using Lab1_5.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab1_5.UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public IActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (user != null && VerifyPassword(model.Password, user.Password))
                {
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    TempData["Message"] = "Login successful!";
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                }
            }

            return View(model);
        }



        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            string hashedPassword = HashPassword(enteredPassword);
            return storedPassword == hashedPassword;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }

}