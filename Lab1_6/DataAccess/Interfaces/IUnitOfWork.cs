using Lab1_6.DataAccess.Repositories;
using Lab1_6.Models.Entity;

namespace Lab1_6.DataAccess.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<User> Users { get; }
    IRepository<Room> Rooms { get; }
    IRepository<Reservation> Reservations { get; }

    int SaveChanges();
}