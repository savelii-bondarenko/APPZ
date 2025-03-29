using Microsoft.AspNetCore.Mvc;
using Lab1_4.Models;
using Lab1_4.Data;
using System.Security.Cryptography;
using System.Text;
using Lab1_4.Models.Entity;

namespace Lab1_4.Controllers;
    public class RegisterController : Controller
    {
        private readonly AppDbContext _context;

        public RegisterController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(RegisterModel model)
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

            var hashedPassword = HashPassword(model.Password); // хешируем пароль

            var user = new User
            {
                Email = model.Email,
                Password = hashedPassword,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            _context.Users.Add(user);
            try
            {
                _context.SaveChanges();
                return RedirectToAction("Index", "Account");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the user. " + ex.Message);
                return View(model);
            }
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
