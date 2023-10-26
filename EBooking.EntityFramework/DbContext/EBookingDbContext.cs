using EBooking.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.EntityFramework.DbContext
{
    internal class EBookingDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<LocationEntity> Locations { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<AdministratorEntity> Administrators { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<AccommodationEntity> Accommodations { get; set; }
        public DbSet<AccommodationUnitEntity> AccommodationUnits { get; set; }
        public DbSet<UnitFeatureEntity> UnitFeatures { get; set; }
        public DbSet<AccommodationUnitReservationEntity> UnitReservations { get; set; }

        public EBookingDbContext(DbContextOptions options) : base(options) { }

        // Granular configuration of model building using FluentAPI
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocationEntity>().HasIndex(x => x.LocationId);
            modelBuilder.Entity<UserEntity>().HasIndex(x => x.UserId);
            modelBuilder.Entity<UserEntity>().HasIndex(x => x.Username).IsUnique(true);
            // TPT - Table per type strategy (Default EF strategy is TPH - Table per hierarchy, with discriminator!)
            modelBuilder.Entity<UserEntity>().UseTptMappingStrategy();
            modelBuilder.Entity<AccommodationEntity>().HasIndex(x => x.AccommodationId);
            modelBuilder.Entity<AccommodationUnitEntity>().HasIndex(x => x.UnitId);
            modelBuilder.Entity<UnitFeatureEntity>().HasIndex(x => x.FeatureId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
