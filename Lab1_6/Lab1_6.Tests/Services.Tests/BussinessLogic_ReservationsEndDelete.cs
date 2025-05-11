using AutoFixture;
using Lab1_6.DataAccess.Interfaces;
using Lab1_6.Models.Entity;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;


namespace TestProject1.Services.Tests;

public class BussinessLogic_ReservationsEndDelete
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly Fixture _fixture = new Fixture();
    private readonly IServiceScopeFactory _serviceScopeFactory = Substitute.For<IServiceScopeFactory>();

    public BussinessLogic_ReservationsEndDelete()
    {
        _fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void Check_ShouldDeleteExpiredReservations()
    {
        var reservations = new List<Reservation>();
    
        var expiredReservation = _fixture.Build<Reservation>()
            .With(r => r.EndDate, DateTime.Now.AddMinutes(-20))
            .Create();
    
        var activeReservation = _fixture.Build<Reservation>()
            .With(r => r.EndDate, DateTime.Now.AddMinutes(20))
            .Create();
    
        reservations.Add(expiredReservation);
        reservations.Add(activeReservation);
    
        _unitOfWork.Reservations.GetAll().Returns(reservations);

        var allReservations = _unitOfWork.Reservations.GetAll();
        foreach (var reservation in allReservations)
        {
            if (reservation.EndDate <= DateTime.Now)
            {
                _unitOfWork.Reservations.Delete(reservation);
            }
        }
        _unitOfWork.SaveChanges();

        _unitOfWork.Reservations.Received(1)
            .Delete(Arg.Is<Reservation>(r => r == expiredReservation));
        _unitOfWork.Reservations.DidNotReceive()
            .Delete(Arg.Is<Reservation>(r => r == activeReservation));
        _unitOfWork.Received(1).SaveChanges();
    }
}