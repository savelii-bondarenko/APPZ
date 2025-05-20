using Lab1_6.BusinessLogic.Services;
using Lab1_6.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(UserService userService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Створити нового користувача", Description = "Реєструє нового користувача та хешує його пароль")]
    public IActionResult Create(User user)
    {
        try
        {
            userService.Create(user);
            return Ok("Користувача успішно створено.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [SwaggerOperation(Summary = "Оновити користувача", Description = "Оновлює дані користувача та, за необхідності, хешує новий пароль")]
    public IActionResult Update(User user)
    {
        try
        {
            userService.Update(user);
            return Ok("Користувача успішно оновлено.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [SwaggerOperation(Summary = "Видалити користувача", Description = "Видаляє користувача за електронною поштою")]
    public IActionResult Delete(User user)
    {
        try
        {
            userService.Delete(user);
            return Ok("Користувача успішно видалено.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{email}")]
    [SwaggerOperation(Summary = "Отримати користувача за email", Description = "Повертає користувача за його електронною адресою")]
    public IActionResult GetByEmail(string email)
    {
        var user = userService.GetByEmail(email);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpGet("id/{id}")]
    [SwaggerOperation(Summary = "Отримати користувача за ID", Description = "Повертає користувача за його унікальним ідентифікатором (GUID)")]
    public IActionResult GetById(Guid id)
    {
        var user = userService.GetById(id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }
}
