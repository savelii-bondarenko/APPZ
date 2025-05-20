namespace Lab1_6.Models.DTOs;

public class RoomUpdateDto
{
    public int Id { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
}
