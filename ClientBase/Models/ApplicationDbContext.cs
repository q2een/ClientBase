using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Models
{
    public class ApplicationDbContext : DbContext
    {
        private readonly LoggerFactory logger = new LoggerFactory(new[] { new DebugLoggerProvider() });

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Founder> Founders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyFounder>()
                        .HasKey(cf => new { cf.FounderId, cf.CompanyId });

            modelBuilder.Entity<Founder>()
                        .Ignore(f => f.FullName);

            modelBuilder.Entity<Founder>()
                        .HasIndex(f => f.TaxpayerId)
                        .IsUnique();

            modelBuilder.Entity<Company>()
                        .HasIndex(c => c.TaxpayerId)
                        .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(logger);
        }
    }
}
