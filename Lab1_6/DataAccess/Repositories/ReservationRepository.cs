using System.Linq.Expressions;
using Lab1_6.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Lab1_6.DataAccess.Repositories
{
    public class ReservationRepository : BaseRepository<Reservation>
    {
        private readonly AppDbContext _context;

        public ReservationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override IEnumerable<Reservation> GetAll()
        {
            return _context.Reservations.Include(r => r.Room)
                .Include(r => r.User)
                .ToList();
        }

        public override Reservation? GetByCondition(Expression<Func<Reservation, bool>> predicate)
        {
            return _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .FirstOrDefault(predicate);
        }

    }
}
