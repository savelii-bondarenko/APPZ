using Lab1_6.DataAccess;

namespace Lab1_6.BusinessLogic.Services;

public class ReservationCleanupService(IServiceProvider serviceProvider, ILogger<ReservationCleanupService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var expiredReservations = dbContext.Reservations
                        .Where(r => r.EndDate < DateTime.Now)
                        .ToList();

                    if (expiredReservations.Any())
                    {
                        foreach (var reservation in expiredReservations)
                        {
                            var room = dbContext.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
                            if (room != null)
                            {
                                room.IsAvailable = true;
                            }

                            dbContext.Reservations.Remove(reservation);
                        }

                        await dbContext.SaveChangesAsync();
                        logger.LogInformation($"Deleted {expiredReservations.Count} of overdue reservations.");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error when deleting reservations: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}
