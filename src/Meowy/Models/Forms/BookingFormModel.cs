using Meowy.Models.Database;

namespace Meowy.Models.Forms;

public record BookingFormModel
{
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public DateTime Date { get; set; }
    public bool IsCallUndesirable { get; set; }

    public static explicit operator Booking(BookingFormModel model) =>
        new()
        {
            Name = model.Name,
            PhoneNumber = model.Phone,
            Date = model.Date,
            IsCallUndesirable = model.IsCallUndesirable
        };
}