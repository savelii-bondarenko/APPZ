    using Lab1_4.Data;

namespace Lab1_4.Services;

public class ReservationCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ReservationCleanupService> _logger;

    public ReservationCleanupService(IServiceProvider serviceProvider, ILogger<ReservationCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
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
                        _logger.LogInformation($"Deleted {expiredReservations.Count} of overdue reservations.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when deleting reservations: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}
