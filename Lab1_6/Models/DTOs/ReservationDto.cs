namespace Lab1_6.Models.DTOs;

public class ReservationDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public int RoomId { get; set; }
    public Guid UserId { get; set; }
}
