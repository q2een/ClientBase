using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Models
{
    public class ApplicationDbContext : DbContext
    {
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
                        .HasIndex(p => p.TaxpayerId)
                        .IsUnique();

            modelBuilder.Entity<Company>()
                        .HasIndex(p => p.TaxpayerId)
                        .IsUnique();
        }
    }
}
