using System.Text.RegularExpressions;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.AspNetCore.Mvc;

namespace Uni.Controllers.Api;

[Route("/api/book")]
[ApiController]
public partial class Book : Controller
{
    private readonly IValidator<Booking> _validator;

    public Book(IValidator<Booking> validator) =>
        _validator = validator;

    public partial record Booking
    {
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime Date { get; set; }
        public string? Message { get; set; }
        public bool IsMessage => Message == "on";

        public partial class Validator : AbstractValidator<Booking>
        {
            [GeneratedRegex(@"^\+?\(?[0-9]{3}\)?[\-\s.]?[0-9]{3}[\-\s.]?[0-9]{4,6}$")]
            private static partial Regex PhoneNumberRegex();

            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
                RuleFor(x => x.Phone).NotEmpty().Matches(PhoneNumberRegex());
                RuleFor(x => x.Date)
                    .NotEmpty()
                    .Must(x =>
                    {
                        DateTimeOffset dto = x;
                        dto = dto.ToOffset(TimeSpan.FromHours(3));
                        return dto.Date >= DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(3)).Date &&
                            dto.TimeOfDay >= new TimeSpan(10, 0, 0) &&
                            dto.TimeOfDay < new TimeSpan(19, 30, 0);
                    });
                RuleFor(x => x.Message).Must(x => x is "on" or "off" or null);
            }
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Post([FromForm] Booking booking)
    {
        ValidationResult validationResult = _validator.Validate(booking);

        if (!validationResult.IsValid)
            return UnprocessableEntity(validationResult.Errors);

        return Ok();
    }
}
