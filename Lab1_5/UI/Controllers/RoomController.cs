using Lab1_5.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab1_5.UI.Controllers;

public class RoomController : Controller
{
    private readonly RoomService _roomService;
    public RoomController(RoomService roomService)
    {
        _roomService = roomService;
    }

    public IActionResult Index()
    {
        var rooms = _roomService.GetAll();
        return View(rooms);
    }
}
