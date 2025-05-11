using Lab1_6.DataAccess.Interfaces;
using Lab1_6.DataAccess.Repositories;
using Lab1_6.Models.Entity;

namespace Lab1_6.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IRepository<User> Users { get; }
    public IRepository<Room> Rooms { get; }
    public IRepository<Reservation> Reservations { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new UserRepository(_context);
        Rooms = new RoomRepository(_context);
        Reservations = new ReservationRepository(_context);
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}