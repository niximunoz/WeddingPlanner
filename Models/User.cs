#pragma warning disable CS8618
#pragma warning disable CS8600
#pragma warning disable CS8602
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WeddingPlanner.Models;
public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [UniqueEmail]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password debe tener al menos 8 caracteres.")]
    public string Password { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [NotMapped]

    [Compare("Password")]
    [DataType(DataType.Password)]
    public string PasswordConfirm { get; set; }

    public List<Attendance> Confirmed { get; set; } = new List<Attendance>();
}

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Email is required");
        }
        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
        if (_context.Users.Any(e => e.Email == value.ToString()))
        {
            return new ValidationResult("El Email ya se encuentra registrado.");
        }

        return ValidationResult.Success;
    }
}


