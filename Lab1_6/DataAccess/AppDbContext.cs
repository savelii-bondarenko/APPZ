using Lab1_6.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Lab1_6.DataAccess
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}