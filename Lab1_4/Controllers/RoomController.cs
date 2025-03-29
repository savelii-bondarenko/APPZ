using Lab1_4.Data;
using Microsoft.AspNetCore.Mvc;

namespace Lab1_4.Controllers;

public class RoomController : Controller
{
    private readonly AppDbContext _context;
    public RoomController(AppDbContext context) { _context = context; }

    //все номеры
    public IActionResult Index()
    {
        var rooms = _context.Rooms.ToList();
        return View(rooms);
    }
}