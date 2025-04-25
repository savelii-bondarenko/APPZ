using Lab1_6.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab1_6.UI.Controllers;

public class AccountController : Controller
{
    private readonly UserService _userService;
    private readonly ReservationService _reservationService;
    private readonly RoomService _roomService;

    public AccountController(UserService userService, ReservationService reservationService, RoomService roomService)
    {
        _userService = userService;
        _reservationService = reservationService;
        _roomService = roomService;
    }

    public IActionResult Index()
    {
        string? userEmail = HttpContext.Session.GetString("UserEmail");

        if (string.IsNullOrEmpty(userEmail))
            return RedirectToAction("Index", "Home");

        var user = _userService.GetByEmail(userEmail);
        if (user == null)
            return RedirectToAction("Index", "Home");

        var reservations = _reservationService.GetByUserEmailWithRooms(userEmail);

        ViewData["FirstName"] = user.FirstName;
        ViewData["LastName"] = user.LastName;
        ViewData["UserEmail"] = user.Email;
        ViewData["Reservations"] = reservations;

        HttpContext.Session.SetString("UserId", user.Id.ToString());

        return View();
    }

    public IActionResult Delete(Guid reservationId)
    {
        var reservation = _reservationService.GetById(reservationId);
        if (reservation != null)
        {
            _reservationService.Delete(reservation);

            var room = _roomService.GetById(reservation.RoomId);
            if (room != null)
            {
                room.IsAvailable = true;
                _roomService.Update(room);
            }
        }

        return RedirectToAction("Index", "Account");
    }
}