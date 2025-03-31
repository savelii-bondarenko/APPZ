using Lab1_4.Data;
using Lab1_4.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Lab1_4.Controllers
{
    public class ReservationController : Controller
    {
        private readonly AppDbContext _context;

        public ReservationController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            HttpContext.Session.SetInt32("RoomId", id);
            return View();
        }

        public IActionResult Create(int roomId)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == roomId);

            if (room == null || !room.IsAvailable)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Account");
            }

            ViewData["RoomId"] = roomId;
            ViewData["UserId"] = userId;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Reservation reservation)
        {

            var room = _context.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
            var user = _context.Users.FirstOrDefault(u => u.Id == reservation.UserId);

            if (room == null || !room.IsAvailable)
            {
                ModelState.AddModelError("", "Room is not available.");
                return View(reservation);
            }

            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View(reservation);
            }

            reservation.Id = Guid.NewGuid();
            reservation.IsActive = true;

            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            room.IsAvailable = false;
            _context.SaveChanges();

            return RedirectToAction("Index", "Account");
        }

    }
}
