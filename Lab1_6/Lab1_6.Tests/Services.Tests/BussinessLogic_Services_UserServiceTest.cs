using System.Linq.Expressions;
using AutoFixture;
using Lab1_6.DataAccess.Interfaces;
using Lab1_6.Models.Entity;
using Microsoft.AspNetCore.Identity;
using NSubstitute;


namespace TestProject1.Services.Tests;

public class BussinessLogic_Services_UserServiceTest
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IPasswordHasher<User> _passwordHasher = Substitute.For<IPasswordHasher<User>>();
    private readonly Fixture _fixture = new Fixture();

    [Fact]
    public void Create_ShouldCreateUser_WhenEmailIsUnique()
    {
        var user = CreateUser();

        _unitOfWork.Users.GetByCondition(Arg.Any<Expression<Func<User, bool>>>())
            .Returns((User)null);

        _passwordHasher.HashPassword(user, Arg.Any<string>())
            .Returns("HashedPassword");

        user.Password = _passwordHasher.HashPassword(user, user.Password);
        _unitOfWork.Users.Create(user);
        _unitOfWork.SaveChanges();

        Assert.Equal("HashedPassword", user.Password);
        _unitOfWork.Users.Received(1).Create(user);
        _unitOfWork.Received(1).SaveChanges();
    }

    [Fact]
    public void Update_ShouldHashPasswordAndUpdateUser_WhenPasswordIsProvided()
    {
         var user = CreateUser();
         
        _unitOfWork.Users.GetByCondition(Arg.Any<Expression<Func<User, bool>>>())
            .Returns(new User());
        _passwordHasher.HashPassword(user, Arg.Any<string>())
            .Returns("hashed");

        if (_unitOfWork.Users.GetByCondition(u => u.Email == user.Email) == null)
        {
            throw new Exception($"Email {user.Email} isn't exists");
        }
        if (!string.IsNullOrEmpty(user.Password))
        {
            user.Password = _passwordHasher.HashPassword(user, user.Password);
        }

        _unitOfWork.Users.Update(user);
        _unitOfWork.SaveChanges();

        Assert.Equal("hashed", user.Password);
        _unitOfWork.Users.Received(1).Update(user);
        _unitOfWork.Received(1).SaveChanges();
    }

    [Fact]
    public void Delete_ShouldDeleteUser_WhenEmailIsProvided()
    {
        var user = CreateUser();
        _unitOfWork.Users.GetByCondition(Arg.Any<Expression<Func<User, bool>>>())
            .Returns(new User());

        if (_unitOfWork.Users.GetByCondition(u => u.Email == user.Email) == null)
        {
            throw new Exception($"Email {user.Email} isn't exists");
        }
        
        _unitOfWork.Users.Delete(user);
        _unitOfWork.SaveChanges();
        
        _unitOfWork.Users.Received(1).Delete(user);
        _unitOfWork.Received(1).SaveChanges();
    }
    
    private User CreateUser()
    {
        var user = _fixture.Build<User>()
            .With(u => u.Email, "test@example.com")
            .Create();
        return user;
    }

    [Fact]
    public void GetById_ShouldReturnUser_WhenIdIsProvided()
    {
        var user = CreateUser(); 
        var userId = user.Id;

        _unitOfWork.Users.GetByCondition(Arg.Any<Expression<Func<User, bool>>>())
            .Returns(user);

        var result = _unitOfWork.Users.GetByCondition(u => u.Id == userId);

        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
    }
    
    [Fact]
    public void GetByEmail_ShouldReturnUser_WhenEmailIsProvided()
    {
        var user = CreateUser(); 
        var userEmail = user.Email;

        _unitOfWork.Users.GetByCondition(Arg.Any<Expression<Func<User, bool>>>())
            .Returns(user);

        var result = _unitOfWork.Users.GetByCondition(u => u.Email == userEmail);

        Assert.NotNull(result);
        Assert.Equal(userEmail, result.Email);
    }
}