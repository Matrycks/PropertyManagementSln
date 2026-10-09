using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Client.Data.Entities;

namespace Client.Data.DbContexts
{
    public class ClientContext : IdentityDbContext<ApplicationUser>
    {
        public ClientContext(
        DbContextOptions<ClientContext> options)
        : base(options)
        {
        }

        public DbSet<RentalProperty> RentalProperties { get; set; }
        public DbSet<FloorPlan> FloorPlans { get; set; }
        public DbSet<RentalUnit> RentalUnits { get; set; }
        public DbSet<RentalApplication> RentalApplications { get; set; }
        public DbSet<ResidenceHistory> ResidenceHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FloorPlan>()
                .Property(f => f.Rent)
                .HasPrecision(18, 2);

            modelBuilder.Entity<RentalUnit>()
            .Property(f => f.Rent)
            .HasPrecision(18, 2);
        }
    }
}