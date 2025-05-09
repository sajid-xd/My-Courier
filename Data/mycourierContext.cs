using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using mycourier.Models;

namespace mycourier.Data
{
    public class mycourierContext : DbContext
    {
        public mycourierContext (DbContextOptions<mycourierContext> options)
            : base(options)
        {
        }

        public DbSet<mycourier.Models.Location> Location { get; set; } = default!;
        public DbSet<mycourier.Models.User> User { get; set; } = default!;
    }
}
