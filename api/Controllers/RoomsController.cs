using Lab1_6.BusinessLogic.Services;
using Lab1_6.Models.DTOs;
using Lab1_6.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController(RoomService roomService) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Отримати всі кімнати", Description = "Повертає список усіх кімнат")]
    public IActionResult GetAll()
    {
        var rooms = roomService.GetAll();
        return Ok(rooms);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Отримати кімнату за ID", Description = "Повертає кімнату за унікальним ідентифікатором")]
    public IActionResult GetById(int id)
    {
        var room = roomService.GetById(id);
        if (room == null)
            return NotFound();
        return Ok(room);
    }

    [HttpPut]
    [SwaggerOperation(Summary = "Оновити кімнату", Description = "Оновлює інформацію про кімнату")]
    public IActionResult Update(RoomUpdateDto roomDto)
    {
        try
        {
            var room = new Room
            {
                Id = roomDto.Id,
                Category = roomDto.Category,
                Price = roomDto.Price,
                IsAvailable = roomDto.IsAvailable
            };

            roomService.Update(room);
            return Ok("Кімнату оновлено.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}