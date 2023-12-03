using System;
using System.Collections.Generic;

namespace Uni.Models.Database;

public partial class User
{
    public long Id { get; set; }

    public string Login { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual UserProfile? UserProfile { get; set; }
}
