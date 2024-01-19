using System;
using System.Collections.Generic;

namespace Meowy.Models.Database;

public partial class News
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public long AuthorId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime Date { get; set; }

    public virtual User Author { get; set; } = null!;
}
