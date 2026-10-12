using System.ComponentModel.DataAnnotations;

namespace CampManager.Models;

public class Accommodation
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Unesi naziv smještaja.")]
    public string Name { get; set; } = "";

    [Range(1, 50, ErrorMessage = "Kapacitet mora biti između 1 i 50.")]
    public int Capacity { get; set; } = 2;

    [Range(1, 10000, ErrorMessage = "Cijena mora biti između 1 i 10000 KM.")]
    public decimal PricePerNight { get; set; } = 100;
}