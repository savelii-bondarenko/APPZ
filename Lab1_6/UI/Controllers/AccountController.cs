using Lab1_6.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab1_6.UI.Controllers;

public class AccountController(UserService userService, ReservationService reservationService, RoomService roomService)
    : Controller
{
    public IActionResult Index()
    {
        string? userEmail = HttpContext.Session.GetString("UserEmail");

        if (string.IsNullOrEmpty(userEmail))
            return RedirectToAction("Index", "Home");

        var user = userService.GetByEmail(userEmail);
        if (user == null)
            return RedirectToAction("Index", "Home");

        var reservations = reservationService.GetByUserEmailWithRooms(userEmail);

        ViewData["FirstName"] = user.FirstName;
        ViewData["LastName"] = user.LastName;
        ViewData["UserEmail"] = user.Email;
        ViewData["Reservations"] = reservations;

        HttpContext.Session.SetString("UserId", user.Id.ToString());

        return View();
    }

    public IActionResult Delete(Guid reservationId)
    {
        var reservation = reservationService.GetById(reservationId);
        if (reservation != null)
        {
            reservationService.Delete(reservation);

            var room = roomService.GetById(reservation.RoomId);
            if (room != null)
            {
                room.IsAvailable = true;
                roomService.Update(room);
            }
        }

        return RedirectToAction("Index", "Account");
    }
}