using FluentValidation;
using FluentValidation.Results;

using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using Uni.Database;
using Uni.Models.Database;
using Uni.Models.Forms;

namespace Uni.Controllers.Api;

[Route("/api/[controller]")]
[ApiController]
public class Bookings(
    IValidator<BookingFormModel> validator,
    UniContext dbContext
    ) : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Post([FromForm] BookingFormModel bookingFormModel)
    {
        ValidationResult validationResult = await validator.ValidateAsync(bookingFormModel);

        if (!validationResult.IsValid)
            return UnprocessableEntity(validationResult.Errors);

        Booking booking = (Booking)bookingFormModel;

        string? currentUserIdString = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (currentUserIdString is not null)
            booking.UserId = Int64.Parse(currentUserIdString);

        if (dbContext.Bookings.Count(x => x.Date == booking.Date) >= 7)
            return NotFound(new { message = "No available for booking tables found." });

        dbContext.Bookings.Add(booking);

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch
        {
            return BadRequest();
        }

        return Ok();
    }
}
