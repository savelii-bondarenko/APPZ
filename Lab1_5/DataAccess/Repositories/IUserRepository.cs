using Lab1_5.Models.Entity;

namespace Lab1_5.DataAccess.Repositories;

public interface IUserRepository
{
    IEnumerable<User> Get();
    void Create(User user);
    void Update(User user);
    void Delete(User user);
}