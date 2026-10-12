using CampManager.Data;
using CampManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampManager.Controllers;

public class AccommodationsController : Controller
{
    private readonly CampDbContext _db;

    public AccommodationsController(CampDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var accommodations = await _db.Accommodations
            .AsNoTracking()
            .OrderBy(a => a.Name)
            .ToListAsync();

        return View(accommodations);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Name,Capacity,PricePerNight")] Accommodation accommodation)
    {
        if (!ModelState.IsValid)
        {
            var accommodations = await _db.Accommodations
                .AsNoTracking()
                .OrderBy(a => a.Name)
                .ToListAsync();

            return View("Index", accommodations);
        }

        _db.Accommodations.Add(accommodation);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var accommodation = await _db.Accommodations.FindAsync(id);

        if (accommodation == null)
        {
            return NotFound();
        }

        var hasReservations = await _db.Reservations
            .AnyAsync(r => r.AccommodationId == id);

        if (hasReservations)
        {
            TempData["Error"] =
                "Smještaj ima rezervacije i ne može se obrisati.";

            return RedirectToAction(nameof(Index));
        }

        _db.Accommodations.Remove(accommodation);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var accommodation = await _db.Accommodations.FindAsync(id);

        if (accommodation == null)
        {
            return NotFound();
        }

        return View(accommodation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Name,Capacity,PricePerNight")] Accommodation accommodation)
    {
        if (id != accommodation.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(accommodation);
        }

        var existing = await _db.Accommodations.FindAsync(id);

        if (existing == null)
        {
            return NotFound();
        }

        existing.Name = accommodation.Name;
        existing.Capacity = accommodation.Capacity;
        existing.PricePerNight = accommodation.PricePerNight;

        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}