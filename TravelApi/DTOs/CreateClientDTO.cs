using System.ComponentModel.DataAnnotations;

namespace TravelApi.DTOs;

public class CreateClientDTO
{
    [Required]
    [StringLength(120)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(120)]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    [StringLength(120)]
    public string Email { get; set; } = null!;

    [Required]
    [Phone]
    [StringLength(120)]
    public string Telephone { get; set; } = null!;

    [Required]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "PESEL must be 11 digits.")]
    public string Pesel { get; set; } = null!;

    public DateTime? PaymentDate { get; set; }
}