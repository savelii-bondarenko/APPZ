using System.Linq.Expressions;

namespace Lab1_6.DataAccess.Repositories;
public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    T? GetByCondition(Expression<Func<T, bool>> predicate);
}