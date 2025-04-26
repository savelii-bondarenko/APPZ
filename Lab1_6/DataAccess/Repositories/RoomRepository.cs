using Lab1_6.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Lab1_6.DataAccess.Repositories;

public class RoomRepository : BaseRepository<Room>
{
    private readonly AppDbContext _context;

    public RoomRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public override IEnumerable<Room> GetAll()
    {
        return _context.Set<Room>().Include(r => r.Reservations).ToList();
    }
}