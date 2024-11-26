using System;
using System.Collections.Generic;

namespace Demo2CoreAPICrud.Models;

public partial class Log
{
    public int LogId { get; set; }

    public DateTime? LogDate { get; set; }

    public Guid? Id { get; set; }

    public bool? Present { get; set; }

    public int? LocationId { get; set; }

    public virtual User? IdNavigation { get; set; }
}
