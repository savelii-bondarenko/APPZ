using System.Linq.Expressions;
using Lab1_5.Models.Entity;

namespace Lab1_5.DataAccess.Repositories;

public class RoomRepository : IRepository<Room>
{
    private readonly AppDbContext _context;

    public RoomRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Room> GetAll()
    {
        return _context.Rooms.ToList();
    }

    public void Create(Room entity)
    {
        _context.Rooms.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Room entity)
    {
        _context.Rooms.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(Room entity)
    {
        _context.Rooms.Remove(entity);
        _context.SaveChanges();
    }

    public Room? GetByCondition(Expression<Func<Room, bool>> predicate)
    {
        return _context.Rooms.FirstOrDefault(predicate);
    }
}