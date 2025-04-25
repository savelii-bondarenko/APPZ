using System.Linq.Expressions;
using Lab1_6.Models.Entity;

namespace Lab1_6.DataAccess.Repositories;

public class UserRepository : IRepository<User>
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<User> GetAll()
    {
        return _context.Users.ToList();
    }

    public User? GetByCondition(Expression<Func<User, bool>> predicate)
    {
        return _context.Users.FirstOrDefault(predicate);
    }

    public void Create(User entity)
    {
        _context.Users.Add(entity);
    }

    public void Update(User entity)
    {
        _context.Users.Update(entity);
    }

    public void Delete(User entity)
    {
        _context.Users.Remove(entity);
    }
}
