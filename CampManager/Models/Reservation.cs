using System.ComponentModel.DataAnnotations;

namespace CampManager.Models;

public class Reservation : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Unesi ime i prezime gosta.")]
    [StringLength(100)]
    public string GuestName { get; set; } = "";

    [Required(ErrorMessage = "Unesi email gosta.")]
    [EmailAddress(ErrorMessage = "Unesi ispravnu email adresu.")]
    [StringLength(200)]
    public string GuestEmail { get; set; } = "";

    [Range(1, int.MaxValue, ErrorMessage = "Izaberi smještaj.")]
    public int AccommodationId { get; set; }

    public Accommodation? Accommodation { get; set; }

    [DataType(DataType.Date)]
    public DateTime CheckIn { get; set; } = DateTime.Today;

    [DataType(DataType.Date)]
    public DateTime CheckOut { get; set; } = DateTime.Today.AddDays(1);

    [Range(1, 50, ErrorMessage = "Broj gostiju mora biti između 1 i 50.")]
    public int NumberOfGuests { get; set; } = 1;

    public decimal TotalPrice { get; set; }

    public IEnumerable<ValidationResult> Validate(
        ValidationContext validationContext)
    {
        if (CheckOut.Date <= CheckIn.Date)
        {
            yield return new ValidationResult(
                "Datum odlaska mora biti nakon datuma dolaska.",
                new[] { nameof(CheckOut) });
        }
    }
}