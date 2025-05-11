using Lab1_6.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab1_6.UI.Controllers;

public class RoomController(RoomService roomService) : Controller
{
    public IActionResult Index()
    {
        var rooms = roomService.GetAll();
        return View(rooms);
    }
}
