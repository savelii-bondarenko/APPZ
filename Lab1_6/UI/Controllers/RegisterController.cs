using Lab1_6.BusinessLogic.Services;
using Lab1_6.Models;
using Lab1_6.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Lab1_6.UI.Controllers;
    public class RegisterController : Controller
    {
        private readonly UserService _userService;
        public RegisterController(UserService userService)
        {
            _userService = userService;
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
                return View(model);


            if (_userService.IsExists(model.Email))
            {
                ModelState.AddModelError("Email", "Email is already taken.");
                return View(model);
            }

            var user = new User
            {
                Email = model.Email,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            try
            {
                _userService.Create(user);
                HttpContext.Session.SetString("UserEmail", user.Email);
                TempData["Message"] = "Login successful!";
                return RedirectToAction("Index", "Account");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the user. " + ex.Message);
                return View(model);
            }
        }

    }
