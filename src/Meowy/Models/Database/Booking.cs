namespace Meowy.Models.Database;

public partial class Booking
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public DateTime Date { get; set; }

    public bool IsCallUndesirable { get; set; }

    public long? UserId { get; set; }

    public virtual User? User { get; set; }
}
