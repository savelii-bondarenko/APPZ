using Lab1_5.DataAccess.Repositories;
using Lab1_5.Models.Entity;

namespace Lab1_5.DataAccess.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<User> Users { get; }
    IRepository<Room> Rooms { get; }
    IRepository<Reservation> Reservations { get; }

    int SaveChanges();
}