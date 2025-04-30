using Lab1_6.BusinessLogic.Services;
using Lab1_6.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Lab1_6.UI.Controllers
{
    public class ReservationController(
        ReservationService reservationService,
        RoomService roomService,
        UserService userService)
        : Controller
    {
        public IActionResult Index(int id)
        {
            HttpContext.Session.SetInt32("RoomId", id);
            return View();
        }

        public IActionResult Create(int roomId)
        {
            var room = roomService.GetById(roomId);

            if (room == null || !room.IsAvailable)
            {
                return NotFound("Room is not available.");
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
            /*
            if (!ModelState.IsValid)
            {
                return View(reservation);
            }
            */

            var room = roomService.GetById(reservation.RoomId);
            var user = userService.GetById(reservation.UserId);

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

            reservationService.Create(reservation);

            room.IsAvailable = false;
            roomService.Update(room);

            return RedirectToAction("Index", "Account");
        }
    }
}
