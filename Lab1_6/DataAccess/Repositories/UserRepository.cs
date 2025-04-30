using Lab1_6.Models.Entity;

namespace Lab1_6.DataAccess.Repositories;

public class UserRepository(AppDbContext context) : BaseRepository<User>(context)
{
    private readonly AppDbContext _context = context;
}
