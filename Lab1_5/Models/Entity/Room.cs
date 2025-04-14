namespace Lab1_5.Models.Entity;

public class Room
{
    public int Id { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}