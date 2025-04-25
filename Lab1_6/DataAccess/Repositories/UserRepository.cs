using System.Linq.Expressions;
using Lab1_6.Models.Entity;

namespace Lab1_6.DataAccess.Repositories;

public class UserRepository : BaseRepository<User>
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

}
