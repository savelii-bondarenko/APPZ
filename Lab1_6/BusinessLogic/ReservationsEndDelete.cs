using Lab1_6.DataAccess.Interfaces;

namespace Lab1_6.BusinessLogic;
public class ReservationsEndDeleteService(IServiceScopeFactory serviceScopeFactory) 
    : IHostedService
{
    private Timer _timer;
    const int CheckIntervalTime = 10;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(PerformTask, null, TimeSpan.Zero, TimeSpan.FromMinutes(CheckIntervalTime));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private void PerformTask(object state)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        Check(unitOfWork);
    }

    private void Check(IUnitOfWork unitOfWork)
    {
        var allReservations = unitOfWork.Reservations.GetAll();
        foreach (var reservation in allReservations)
        {
            if (reservation.EndDate <= DateTime.Now)
            {
                unitOfWork.Reservations.Delete(reservation);
            }
        }
        unitOfWork.SaveChanges();
    }
}