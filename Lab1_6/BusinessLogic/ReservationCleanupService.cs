using Lab1_6.DataAccess.Interfaces;

namespace Lab1_6.BusinessLogic;

public class ReservationCleanupService(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(15);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await CleanupExpiredReservations();
            await Task.Delay(_interval, stoppingToken);
        }
    }

    public async Task CleanupExpiredReservations()
    {
        using var scope = serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var expiredReservations = unitOfWork.Reservations
            .GetAll()
            .Where(r => r.EndDate <= DateTime.UtcNow)
            .ToList();

        foreach (var reservation in expiredReservations)
        {
            unitOfWork.Reservations.Delete(reservation);

            var room = unitOfWork.Rooms.GetByCondition(r => r.Id == reservation.RoomId);
            if (room != null)
            {
                room.IsAvailable = true;
                unitOfWork.Rooms.Update(room);
            }
        }

        if (expiredReservations.Any())
        {
            unitOfWork.SaveChanges();
        }

        await Task.CompletedTask;
    }
}
