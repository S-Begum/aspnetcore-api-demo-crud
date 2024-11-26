using System;
using System.Collections.Generic;

namespace Demo2CoreAPICrud.Models;

public partial class Location
{
    public int LocationId { get; set; }

    public string Company { get; set; } = null!;

    public string Building { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? Country { get; set; }
}
