using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Model;
using Microsoft.EntityFrameworkCore;

namespace API.DAL
{
    public class SpelContext : DbContext
    {
        public SpelContext(DbContextOptions options) : base(options) { }

        public DbSet<Spel> Spellen { get; set; }

    }
}
