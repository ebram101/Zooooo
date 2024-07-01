using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dierentuin.Models;

namespace Zoo.Data
{
    public class ZooContext : DbContext
    {
        public ZooContext (DbContextOptions<ZooContext> options)
            : base(options)
        {
        }

        public DbSet<Dierentuin.Models.Animals> Animals { get; set; } = default!;
        public DbSet<Dierentuin.Models.Category> Category { get; set; } = default!;
        public DbSet<Dierentuin.Models.Enclosure> Enclosure { get; set; } = default!;
    }
}
