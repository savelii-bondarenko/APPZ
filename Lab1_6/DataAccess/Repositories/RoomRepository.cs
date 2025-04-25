using System.Linq.Expressions;
using Lab1_6.Models.Entity;

namespace Lab1_6.DataAccess.Repositories;

public class RoomRepository : BaseRepository<Room>
{
    private readonly AppDbContext _context;

    public RoomRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }


}