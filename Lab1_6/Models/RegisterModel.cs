using System.ComponentModel.DataAnnotations;

namespace Lab1_6.Models;

public class RegisterModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Confirm password is required.")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
    public string ConfirmPassword { get; set; } = null!;

    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    [RegularExpression("^[A-Za-zА-Яа-яІіЇїЄєҐґ'-]+$", ErrorMessage = "First name can only contain letters.")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    [RegularExpression("^[A-Za-zА-Яа-яІіЇїЄєҐґ'-]+$", ErrorMessage = "Last name can only contain letters.")]
    public string LastName { get; set; } = null!;
}