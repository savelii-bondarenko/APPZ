using System.Linq.Expressions;

namespace Lab1_6.DataAccess.Repositories;
public class BaseRepository<T>(AppDbContext context) : IRepository<T>
    where T : class
{
    public virtual IEnumerable<T> GetAll()
    {
        return context.Set<T>().ToList();
    }

    public void Create(T entity)
    {
        context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        context.Set<T>().Remove(entity);
    }
    
    public virtual T? GetByCondition(Expression<Func<T, bool>> predicate)
    {
        return context.Set<T>().FirstOrDefault(predicate);
    }
}