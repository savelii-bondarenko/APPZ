using Lab1_5.Models.Entity;

namespace Lab1_5.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private UserRepository(AppDbContext context) => _context = context;

    public IEnumerable<User> Get() => _context.Users;

    public void Create(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void Delete(User user)
    {
        _context.Users.Remove(user);
        _context.SaveChanges();
    }
}