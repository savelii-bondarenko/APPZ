using Lab1_6.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Lab1_6.DataAccess.Repositories;

public class RoomRepository(AppDbContext context) : BaseRepository<Room>(context)
{
    private readonly AppDbContext _context = context;

    public override IEnumerable<Room> GetAll()
    {
        return _context.Set<Room>().Include(r => r.Reservations).ToList();
    }
}