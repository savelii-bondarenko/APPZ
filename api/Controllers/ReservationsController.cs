using Lab1_6.BusinessLogic.Services;
using Lab1_6.Models.DTOs;
using Lab1_6.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly ReservationService _reservationService;

    public ReservationsController(ReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Створити нову резервацію", Description = "Створює нову резервацію з посиланнями на кімнату та користувача")]
    public IActionResult Create([FromBody] ReservationDto dto)
    {
        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            IsActive = dto.IsActive,
            RoomId = dto.RoomId,
            UserId = dto.UserId
        };

        try
        {
            _reservationService.Create(reservation);
            return Ok("Резервацію успішно створено.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Оновити резервацію", Description = "Оновлює існуючу резервацію")]
    public IActionResult Update(Guid id, [FromBody] ReservationDto dto)
    {
        var existing = _reservationService.GetById(id);
        if (existing == null)
            return NotFound($"Резервація з id={id} не знайдена.");

        existing.StartDate = dto.StartDate;
        existing.EndDate = dto.EndDate;
        existing.IsActive = dto.IsActive;
        existing.RoomId = dto.RoomId;
        existing.UserId = dto.UserId;

        try
        {
            _reservationService.Update(existing);
            return Ok("Резервацію успішно оновлено.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Видалити резервацію", Description = "Видаляє резервацію за її ідентифікатором")]
    public IActionResult Delete(Guid id)
    {
        var existing = _reservationService.GetById(id);
        if (existing == null)
            return NotFound($"Резервація з id={id} не знайдена.");

        try
        {
            _reservationService.Delete(existing);
            return Ok("Резервацію успішно видалено.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Отримати резервацію за ID", Description = "Повертає резервацію за її унікальним ідентифікатором")]
    public IActionResult GetById(Guid id)
    {
        var reservation = _reservationService.GetById(id);
        if (reservation == null)
            return NotFound();

        var dto = MapToDto(reservation);
        return Ok(dto);
    }

    [HttpGet("user/{email}")]
    [SwaggerOperation(Summary = "Отримати резервації користувача за email", Description = "Повертає список резервацій для користувача за його email")]
    public IActionResult GetByUserEmail(string email)
    {
        var reservations = _reservationService.GetByUserEmailWithRooms(email);

        var dtos = reservations.Select(MapToDto);
        return Ok(dtos);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Отримати всі резервації", Description = "Повертає список всіх резервацій")]
    public IActionResult GetAll()
    {
        var reservations = _reservationService.GetAll();

        var dtos = reservations.Select(MapToDto);
        return Ok(dtos);
    }

    private static ReservationDto MapToDto(Reservation reservation)
    {
        return new ReservationDto
        {
            StartDate = reservation.StartDate,
            EndDate = reservation.EndDate,
            IsActive = reservation.IsActive,
            RoomId = reservation.RoomId,
            UserId = reservation.UserId
        };
    }
}
