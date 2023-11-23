using System;
using System.Collections.Generic;

namespace Uni.Models.Database;

public partial class Event
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public byte[] Description { get; set; } = null!;

    public long AuthorId { get; set; }

    public virtual User Author { get; set; } = null!;

    public virtual ICollection<User> Participants { get; set; } = new List<User>();
}
