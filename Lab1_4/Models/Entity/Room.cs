namespace Lab1_4.Models.Entity;

public class Room
{
    public int Id { get; set; }
    public bool IsAvailable { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
    public virtual ICollection<Reservation> Reservations { get; set; }
}
