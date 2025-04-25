using Lab1_6.DataAccess;
using Lab1_6.DataAccess.Interfaces;
using Lab1_6.Models.Entity;
using Microsoft.AspNetCore.Identity;

namespace Lab1_6.BusinessLogic.Services;

public class UserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IPasswordHasher<User> passwordHasher, IUnitOfWork unitOfWork)
    {
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public void Create(User entity)
    {
        if (IsExists(entity.Email))
            throw new Exception($"Email {entity.Email} already exists");

        entity.Password = _passwordHasher.HashPassword(entity, entity.Password);
        _unitOfWork.Users.Create(entity);
        _unitOfWork.SaveChanges();
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

        _unitOfWork.Users.Update(entity);
        _unitOfWork.SaveChanges();

    }

    public void Delete(User entity)
    {
        if (!IsExists(entity.Email))
        {
            throw new Exception($"Email {entity.Email} isn't exists");
        }

        _unitOfWork.Users.Delete(entity);
        _unitOfWork.SaveChanges();

    }

    public bool IsExists(string email)
    {
        var user = _unitOfWork.Users.GetByCondition(u => u.Email == email);
        return user != null;
    }

    public User? GetByEmail(string email)
    {
        return _unitOfWork.Users.GetByCondition(u => u.Email == email);
    }

    public User? GetById(Guid id)
    {
        return _unitOfWork.Users.GetByCondition(u => u.Id == id);
    }

}