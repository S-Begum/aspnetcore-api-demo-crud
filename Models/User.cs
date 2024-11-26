using System;
using System.Collections.Generic;

namespace Demo2CoreAPICrud.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
}
