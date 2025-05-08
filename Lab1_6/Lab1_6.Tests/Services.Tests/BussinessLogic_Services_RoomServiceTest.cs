using AutoFixture;
using Lab1_6.DataAccess.Interfaces;
using Lab1_6.Models.Entity;
using NSubstitute;

public class BussinessLogic_Services_RoomServiceTest
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly Fixture _fixture;

    public BussinessLogic_Services_RoomServiceTest()
    {
        _fixture = new Fixture();
        _fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
    
    [Fact]
    public void GetAll_ShouldReturnAllRooms()
    {
        var rooms = _fixture.CreateMany<Room>(10).ToList();
        _unitOfWork.Rooms.GetAll().Returns(rooms);

        var result = _unitOfWork.Rooms.GetAll();

        Assert.Equal(rooms.Count, result.Count());
        Assert.Equal(rooms, result);
    }
}