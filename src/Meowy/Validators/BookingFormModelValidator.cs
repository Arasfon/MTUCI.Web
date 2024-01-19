using FluentValidation;

using Meowy.Models.Forms;

using System.Text.RegularExpressions;

namespace Meowy.Validators;

public partial class BookingFormModelValidator : AbstractValidator<BookingFormModel>
{
    [GeneratedRegex(@"^\+?\(?[0-9]{3}\)?[\-\s.]?[0-9]{3}[\-\s.]?[0-9]{4,6}$")]
    private static partial Regex PhoneNumberRegex();

    public BookingFormModelValidator()
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
    }
}
