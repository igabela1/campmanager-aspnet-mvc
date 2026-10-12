using System.Data;
using CampManager.Data;
using CampManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CampManager.Controllers;

public class ReservationsController : Controller
{
    private readonly CampDbContext _db;

    public ReservationsController(CampDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var reservations = await _db.Reservations
            .AsNoTracking()
            .Include(r => r.Accommodation)
            .OrderByDescending(r => r.CheckIn)
            .ToListAsync();

        return View(reservations);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadAccommodations();

        return View(new Reservation());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("GuestName,GuestEmail,AccommodationId,CheckIn,CheckOut,NumberOfGuests")]
        Reservation reservation)
    {
        reservation.CheckIn = reservation.CheckIn.Date;
        reservation.CheckOut = reservation.CheckOut.Date;

        if (!ModelState.IsValid)
        {
            await LoadAccommodations(reservation.AccommodationId);
            return View(reservation);
        }

        await using var transaction = await _db.Database
            .BeginTransactionAsync(IsolationLevel.Serializable);

        var accommodation = await _db.Accommodations
            .FindAsync(reservation.AccommodationId);

        if (accommodation == null)
        {
            ModelState.AddModelError(
                nameof(Reservation.AccommodationId),
                "Izabrani smještaj ne postoji.");
        }
        else if (reservation.NumberOfGuests > accommodation.Capacity)
        {
            ModelState.AddModelError(
                nameof(Reservation.NumberOfGuests),
                $"Smještaj prima najviše {accommodation.Capacity} osoba.");
        }

        var overlaps = await _db.Reservations.AnyAsync(r =>
            r.AccommodationId == reservation.AccommodationId &&
            r.CheckIn < reservation.CheckOut &&
            reservation.CheckIn < r.CheckOut);

        if (overlaps)
        {
            ModelState.AddModelError(
                "",
                "Smještaj je već rezervisan u izabranom periodu.");
        }

        if (!ModelState.IsValid)
        {
            await transaction.RollbackAsync();
            await LoadAccommodations(reservation.AccommodationId);
            return View(reservation);
        }

        var nights = (reservation.CheckOut - reservation.CheckIn).Days;

        reservation.TotalPrice = nights * accommodation!.PricePerNight;

        _db.Reservations.Add(reservation);
        await _db.SaveChangesAsync();
        await transaction.CommitAsync();

        TempData["Success"] = "Rezervacija je uspješno dodana.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var reservation = await _db.Reservations.FindAsync(id);

        if (reservation == null)
        {
            return NotFound();
        }

        _db.Reservations.Remove(reservation);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Rezervacija je obrisana.";

        return RedirectToAction(nameof(Index));
    }

    private async Task LoadAccommodations(int? selectedId = null)
    {
        var accommodations = await _db.Accommodations
            .AsNoTracking()
            .OrderBy(a => a.Name)
            .ToListAsync();

        ViewBag.Accommodations = new SelectList(
            accommodations,
            "Id",
            "Name",
            selectedId);
    }
}