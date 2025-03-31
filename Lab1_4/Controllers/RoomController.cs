using Lab1_4.Data;
using Lab1_4.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Lab1_4.Controllers;

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
