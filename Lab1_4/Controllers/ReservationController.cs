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

    }
}
