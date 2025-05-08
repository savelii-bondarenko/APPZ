using System.Linq.Expressions;
using AutoFixture;
using Lab1_6.BusinessLogic;
using Lab1_6.DataAccess.Interfaces;
using Lab1_6.Models.Entity;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

public class BussinessLogic_ReservationCleanupService
{
    private readonly Fixture _fixture = new();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IServiceScope _scope;
    private readonly IServiceProvider _serviceProvider;

    public BussinessLogic_ReservationCleanupService()
    {
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_unitOfWork); 
        serviceCollection.AddScoped<ReservationCleanupService>();

        _serviceProvider = serviceCollection.BuildServiceProvider();

        _scope = _serviceProvider.CreateScope();
    }

    [Fact]
    public async Task CleanupExpiredReservations_ShouldDeleteExpiredReservations_AndFreeRooms()
    {
        var expiredReservation = _fixture.Build<Reservation>()
            .With(r => r.EndDate, DateTime.UtcNow.AddMinutes(-10))
            .Create();

        var associatedRoom = _fixture.Build<Room>()
            .With(r => r.Id, expiredReservation.RoomId)
            .With(r => r.IsAvailable, false)
            .Create();

        _unitOfWork.Reservations.GetAll().Returns(new[] { expiredReservation });
        _unitOfWork.Rooms.GetByCondition(Arg.Any<Expression<Func<Room, bool>>>())
            .Returns(associatedRoom);

        var service = _scope.ServiceProvider.GetRequiredService<ReservationCleanupService>();

        await service.CleanupExpiredReservations();

        _unitOfWork.Reservations.Received(1).Delete(expiredReservation);
        _unitOfWork.Rooms.Received(1).Update(associatedRoom);
        Assert.True(associatedRoom.IsAvailable);
        _unitOfWork.Received(1).SaveChanges();
    }
}
