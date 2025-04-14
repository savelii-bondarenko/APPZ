using System.Linq.Expressions;

namespace Lab1_5.DataAccess.Repositories;

public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    T? GetByCondition(Expression<Func<T, bool>> predicate);

}