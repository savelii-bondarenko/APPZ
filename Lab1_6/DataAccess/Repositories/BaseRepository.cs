using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Lab1_6.DataAccess.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public virtual IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public void Create(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public virtual T? GetByCondition(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().FirstOrDefault(predicate);
    }
}