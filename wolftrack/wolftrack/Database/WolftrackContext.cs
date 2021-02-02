using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wolftrack.Models;

namespace wolftrack.Database
{
    public class WolftrackContext : DbContext
    {
       //public WolftrackContext(DbContextOptions<WolftrackContext> options) : base(options) { }
        public DbSet<Wolf> Wolves { get; set; }
        public DbSet<Pack> Packs { get; set; }
        public readonly static string connectionString = "Server =localhost; " +
            "Database =wolftrack; " +
            "User Id =sa; " +
            "Password =admin!@#123;";

        protected override void OnConfiguring
            (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

    }
}
