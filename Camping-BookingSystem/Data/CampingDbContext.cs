using BookingSystem_ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem_ClassLibrary.Data
{
    public class CampingDbContext : DbContext
    {
        public DbSet<CampSite> CampSites { get; set; }
        public DbSet<CampSpot> CampSpots { get; set; }
        public DbSet<SpotType> SpotTypes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public CampingDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // SpotTypes
            modelBuilder.Entity<SpotType>().HasData(
                new SpotType { Id = 1, Name = "Tent", Price = 150 },
                new SpotType { Id = 2, Name = "Caravan", Price = 250 },
                new SpotType { Id = 3, Name = "Cabin", Price = 500 }
            );

            // CampSites
            modelBuilder.Entity<CampSite>().HasData(
                new CampSite { Id = 1, Name = "Solgläntan", Adress = "Solvägen 1", Description = "Skogsnära camping med badsjö" },
                new CampSite { Id = 2, Name = "Havsutsikten", Adress = "Strandvägen 2", Description = "Havsnära camping med aktiviteter" }
            );

            // CampSpots
            modelBuilder.Entity<CampSpot>().HasData(
                new CampSpot { Id = 1, CampSiteId = 1, TypeId = 1, Electricity = false, MaxPersonLimit = 4 },
                new CampSpot { Id = 2, CampSiteId = 1, TypeId = 2, Electricity = true, MaxPersonLimit = 6 },
                new CampSpot { Id = 3, CampSiteId = 2, TypeId = 3, Electricity = true, MaxPersonLimit = 5 }
            );

            // Customers
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    FirstName = "Anna",
                    LastName = "Andersson",
                    Email = "anna.andersson@example.com",
                    PhoneNumber = "0701234567",
                    StreetAddress = "Sommargatan 12",
                    ZipCode = "43245",
                    City = "Varberg"
                },
                new Customer
                {
                    Id = 2,
                    FirstName = "Björn",
                    LastName = "Bergström",
                    Email = "bjorn.bergstrom@example.com",
                    PhoneNumber = "0739876543",
                    StreetAddress = "Campingvägen 8",
                    ZipCode = "12345",
                    City = "Halmstad"
                }
            );

            // Bookings
            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    Id = 1,
                    CustomerId = 1,
                    CampSpotId = 1,
                    StartDate = new DateTime(2025, 6, 15),
                    EndDate = new DateTime(2025, 6, 20),
                    NumberOfPeople = 2
                },
                new Booking
                {
                    Id = 2,
                    CustomerId = 2,
                    CampSpotId = 3,
                    StartDate = new DateTime(2025, 7, 1),
                    EndDate = new DateTime(2025, 7, 7),
                    NumberOfPeople = 4
                }
            );
        }
    }
}
