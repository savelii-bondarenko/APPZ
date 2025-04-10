using Lab1_5.DataAccess.Repositories;
using Lab1_5.Models.Entity;
using Microsoft.AspNetCore.Identity;

namespace Lab1_5.BusinessLogic.Services;

public class UserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IRepository<User> userRepository, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public IEnumerable<User> Get()
    {
        throw new NotImplementedException();
    }

    public void Create(User entity)
    {
        if (IsExists(entity.Email))
            throw new Exception($"Email {entity.Email} already exists");

        entity.Password = _passwordHasher.HashPassword(entity, entity.Password);
        _userRepository.Create(entity);
    }

    public void Update(User entity)
    {
        if (!IsExists(entity.Email))
        {
            throw new Exception($"Email {entity.Email} isn't exists");
        }

        if (!string.IsNullOrEmpty(entity.Password))
        {
            entity.Password = _passwordHasher.HashPassword(entity, entity.Password);
        }

        _userRepository.Update(entity);
    }

    public void Delete(User entity)
    {
        if (!IsExists(entity.Email))
        {
            throw new Exception($"Email {entity.Email} isn't exists");
        }

        _userRepository.Delete(entity);
    }

    public bool IsExists(string email)
    {
        var user = _userRepository.GetByCondition(u => u.Email == email);
        return user != null;
    }

    public User? GetByEmail(string email)
    {
        return _userRepository.GetByCondition(u => u.Email == email);
    }

    public User? GetById(Guid id)
    {
        return _userRepository.GetByCondition(u => u.Id == id);
    }

}