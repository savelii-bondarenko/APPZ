using System.Linq.Expressions;
using Lab1_6.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Lab1_6.DataAccess.Repositories
{
    public class ReservationRepository : IRepository<Reservation>
    {
        private readonly AppDbContext _context;

        public ReservationRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Reservation> GetAll()
        {
            return _context.Reservations.Include(r => r.Room)
                .Include(r => r.User)
                .ToList();
        }

        public void Create(Reservation entity)
        {
            _context.Reservations.Add(entity);
        }

        public void Update(Reservation entity)
        {
            _context.Reservations.Update(entity);
        }

        public void Delete(Reservation entity)
        {
            _context.Reservations.Remove(entity);
        }

        public Reservation? GetByCondition(Expression<Func<Reservation, bool>> predicate)
        {
            return _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .FirstOrDefault(predicate);
        }

    }
}
