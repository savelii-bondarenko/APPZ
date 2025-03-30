using Lab1_4.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab1_4.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        string? userEmail = HttpContext.Session.GetString("UserEmail");

        if (string.IsNullOrEmpty(userEmail))
            return RedirectToAction("Index", "Home");

        var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

        if (user == null)
            return RedirectToAction("Index", "Home");

        var reservations = _context.Reservations
            .Where(r => r.User.Email == userEmail)
            .Include(r => r.Room)
            .ToList();

        ViewData["FirstName"] = user.FirstName;
        ViewData["LastName"] = user.LastName;
        ViewData["UserEmail"] = user.Email;
        ViewData["Reservations"] = reservations;
        HttpContext.Session.SetString("UserId", user.Id.ToString());


        return View();
    }


    public IActionResult Delete(Guid reservationId)
    {
        var reservation = _context.Reservations.FirstOrDefault(r => r.Id == reservationId);

        if (reservation != null)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);

            _context.Reservations.Remove(reservation);
            _context.SaveChanges();

            if (room != null)
            {
                room.IsAvailable = true;
                _context.SaveChanges();
            }
        }

        return RedirectToAction("Index", "Account");
    }

}