using System;
using System.Collections.Generic;

namespace mycourier.Models;

public partial class Location
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Fees { get; set; }
}
