using System.ComponentModel.DataAnnotations;

namespace Lab1_5.Models.Entity;

public class Reservation
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Start Date is required.")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End Date is required.")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }

    [Required(ErrorMessage = "Room is required.")]
    public int RoomId { get; set; }

    [Required(ErrorMessage = "User is required.")]
    public Guid UserId { get; set; }

    public virtual Room Room { get; set; }
    public virtual User User { get; set; }
}

