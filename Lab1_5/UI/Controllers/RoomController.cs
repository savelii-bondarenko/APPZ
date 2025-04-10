using Lab1_5.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Lab1_5.UI.Controllers;

public class RoomController : Controller
{
    private readonly AppDbContext _context;
    public RoomController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var rooms = _context.Rooms.ToList();
        return View(rooms);
    }
}
