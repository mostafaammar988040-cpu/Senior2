using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Senior2.Api.Data;
using Senior2.Api.DTOs.Advertisements;
using Senior2.Api.Extensions;
using Senior2.Api.Models;

namespace Senior2.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AdvertisementController : ControllerBase
{
    private readonly AppDbContext _context;

    public AdvertisementController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpPost]
    [Authorize]
    [HttpPost]
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<AdvertisementDto>> Create(
    [FromForm] CreateAdvertisementDto request,
    CancellationToken cancellationToken)
    {
        if (request is null)
            return BadRequest("Request body is required.");

        if (request.EndDateUtc <= request.StartDateUtc)
        {
            ModelState.AddModelError(nameof(request.EndDateUtc), "EndDateUtc must be greater than StartDateUtc.");
            return ValidationProblem(ModelState);
        }

        if (request.ImageFile == null || request.ImageFile.Length == 0)
        {
            return BadRequest("Advertisement image is required.");
        }

        string? imageUrl = null;

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var extension = Path.GetExtension(request.ImageFile.FileName).ToLower();

        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest("Only JPG, JPEG, PNG, and WEBP images are allowed.");
        }

        var uploadsFolder = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "uploads",
            "ads"
        );

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.ImageFile.CopyToAsync(stream, cancellationToken);
        }

        imageUrl = $"/uploads/ads/{fileName}";

        var ad = new Advertisement
        {
            PlaceId = null,
            StartDateUtc = request.StartDateUtc,
            EndDateUtc = request.EndDateUtc,
            Priority = request.Priority,
            AdminNote = request.AdminNote,
            ImageUrl = imageUrl,
            Status = AdvertisementStatus.Pending,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        _context.Advertisements.Add(ad);
        await _context.SaveChangesAsync(cancellationToken);

        return Created($"/api/advertisement/{ad.Id}", MapToDto(ad));
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<AdvertisementDto>> Update(int id, [FromBody] UpdateAdvertisementDto request, CancellationToken cancellationToken)
    {
        if (request is null) return BadRequest("Request body is required.");
        if (id != request.Id) return BadRequest("Route id and body id do not match.");

        if (request.EndDateUtc <= request.StartDateUtc)
        {
            ModelState.AddModelError(nameof(request.EndDateUtc), "EndDateUtc must be greater than StartDateUtc.");
            return ValidationProblem(ModelState);
        }

        var ad = await _context.Advertisements.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (ad is null) return NotFound();

        ad.StartDateUtc = request.StartDateUtc;
        ad.EndDateUtc = request.EndDateUtc;
        ad.Priority = request.Priority;
        ad.AdminNote = request.AdminNote;
        ad.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        ad = await _context.Advertisements
            .Include(a => a.Place)
            .FirstAsync(a => a.Id == id, cancellationToken);

        return Ok(MapToDto(ad));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}/status")]
    public async Task<ActionResult<AdvertisementDto>> UpdateStatus(int id, [FromBody] UpdateAdvertisementStatusDto request, CancellationToken cancellationToken)
    {
        if (request is null) return BadRequest("Request body is required.");
        if (id != request.Id) return BadRequest("Route id and body id do not match.");

        var allowedStatuses = new[]
        {
            AdvertisementStatus.Approved,
            AdvertisementStatus.Rejected,
            AdvertisementStatus.Paused
        };

        if (!allowedStatuses.Contains(request.Status))
        {
            return BadRequest("Only Approved, Rejected, or Paused are allowed.");
        }

        var ad = await _context.Advertisements.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (ad is null) return NotFound();

        ad.Status = request.Status;
        ad.AdminNote = request.AdminNote;
        ad.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        ad = await _context.Advertisements
            .Include(a => a.Place)
            .FirstAsync(a => a.Id == id, cancellationToken);

        return Ok(MapToDto(ad));
    }

    [AllowAnonymous]
    [HttpGet("active")]
    public async Task<ActionResult<IReadOnlyList<AdvertisementDto>>> GetActive(CancellationToken cancellationToken)
    {
        var ads = await _context.Advertisements
            .AsNoTracking()
            .Include(a => a.Place)
            .Active()
            .OrderByDescending(a => a.Priority)
            .ThenByDescending(a => a.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        var result = ads.Select(MapToDto).ToList();
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<ActionResult<List<AdvertisementDto>>> GetAll(CancellationToken cancellationToken)
    {
        var ads = await _context.Advertisements
            .AsNoTracking()
            .Include(a => a.Place)
            .OrderByDescending(a => a.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return Ok(ads.Select(MapToDto).ToList());
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var ad = await _context.Advertisements.FindAsync(new object[] { id }, cancellationToken);
        if (ad is null) return NotFound();

        _context.Advertisements.Remove(ad);
        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    private static AdvertisementDto MapToDto(Advertisement ad)
    {
        var now = DateTimeOffset.UtcNow;

        return new AdvertisementDto
        {
            Id = ad.Id,

            PlaceId = ad.PlaceId ?? 0,

            PlaceName = ad.Place?.Name
                        ?? ad.AdminNote
                        ?? "Custom Advertisement",

            ImageUrl = !string.IsNullOrWhiteSpace(ad.ImageUrl)
                ? ad.ImageUrl
                : ad.Place?.ImageUrl ?? string.Empty,

            StartDateUtc = ad.StartDateUtc,
            EndDateUtc = ad.EndDateUtc,
            Priority = ad.Priority,
            Status = ad.Status,
            AdminNote = ad.AdminNote,
            IsActive = ad.Status == AdvertisementStatus.Approved
                       && ad.StartDateUtc <= now
                       && ad.EndDateUtc >= now
        };
    }
}