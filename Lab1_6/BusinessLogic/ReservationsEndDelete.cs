using Lab1_6.DataAccess.Interfaces;
using Lab1_6.Models.Entity;

namespace Lab1_6.BusinessLogic;

public class ReservationsEndDeleteService(IUnitOfWork unitOfWork) : IHostedService
{
    private Timer _timer;
    const int CheckIntervalTime = 10;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(PerformTask, null, TimeSpan.Zero, 
            TimeSpan.FromMinutes(CheckIntervalTime));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private void PerformTask(object state)
    {
        Check();
    }

    private void Delete(Reservation reservation)
    {
        unitOfWork.Reservations.Delete(reservation);
        unitOfWork.SaveChanges();
    }

    private void Check()
    {
        var allReservations = unitOfWork.Reservations.GetAll();

        foreach (var reservation in allReservations)
        {
            if (reservation.EndDate <= DateTime.Now)
            {
                Delete(reservation);
            }
        }
    }
}