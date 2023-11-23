using System;
using System.Collections.Generic;

namespace Uni.Models.Database;

public partial class News
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public byte[] Content { get; set; } = null!;

    public DateTimeOffset Date { get; set; }

    public long AuthorId { get; set; }

    public virtual User Author { get; set; } = null!;
}
