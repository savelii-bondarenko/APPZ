using Lab1_6.DataAccess.Interfaces;
using Lab1_6.Models.Entity;
using Microsoft.AspNetCore.Identity;

namespace Lab1_6.BusinessLogic.Services;

public class UserService(IPasswordHasher<User> passwordHasher, IUnitOfWork unitOfWork)
{
    public void Create(User entity)
    {
        if (IsExists(entity.Email))
            throw new Exception($"Email {entity.Email} already exists");

        entity.Password = passwordHasher.HashPassword(entity, entity.Password);
        unitOfWork.Users.Create(entity);
        unitOfWork.SaveChanges();
    }

    public void Update(User entity)
    {
        var existingUser = unitOfWork.Users.GetByCondition(u => u.Id == entity.Id);
        if (existingUser == null)
        {
            throw new Exception($"User with ID {entity.Id} does not exist");
        }

        existingUser.Email = entity.Email;
        if (!string.IsNullOrEmpty(entity.Password))
        {
            existingUser.Password = passwordHasher.HashPassword(existingUser, entity.Password);
        }

        unitOfWork.Users.Update(existingUser);
        unitOfWork.SaveChanges();
    }


    public void Delete(User entity)
    {
        var existingUser = unitOfWork.Users.GetByCondition(u => u.Id == entity.Id);
        if (existingUser == null)
        {
            throw new Exception($"User with ID {entity.Id} does not exist");
        }

        unitOfWork.Users.Delete(existingUser);
        unitOfWork.SaveChanges();
    }


    public bool IsExists(string email)
    {
        var user = unitOfWork.Users.GetByCondition(u => u.Email == email);
        return user != null;
    }

    public User? GetByEmail(string email)
    {
        return unitOfWork.Users.GetByCondition(u => u.Email == email); 
    }

    public User? GetById(Guid id)
    {
        return unitOfWork.Users.GetByCondition(u => u.Id == id); 
    }

}