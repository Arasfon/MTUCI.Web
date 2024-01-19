namespace Meowy.Models.Database;

public partial class UserProfile
{
    public long UserId { get; set; }

    public string? Bio { get; set; }

    public virtual User User { get; set; } = null!;
}
