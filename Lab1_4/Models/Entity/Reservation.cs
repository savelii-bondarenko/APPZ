using Lab1_4.Models.Entity;

public class Reservation
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public int RoomId { get; set; }
    public Guid UserId { get; set; }

    public virtual Room Room { get; set; }
    public virtual User User { get; set; }
}