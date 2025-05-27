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


            // Var tvungen att lägga till denna för att få priset att visas korrekt, EF förstod inte att det
            //fanns någon relation mellan SpotType.Id och TypeId, "SpotTypeId" förväntades. det var antingen detta eller ändra namnet överallt.
            modelBuilder.Entity<CampSpot>()
                .HasOne(cs => cs.SpotType)
                .WithMany(st => st.CampSpots)
                .HasForeignKey(cs => cs.TypeId);

        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // SpotTypes
            modelBuilder.Entity<SpotType>().HasData(
                new SpotType { Id = 1, Name = "Tent", Price = 150, MaxPersonLimit = 8 },
                new SpotType { Id = 2, Name = "Caravan", Price = 250, MaxPersonLimit = 8 },
                new SpotType { Id = 3, Name = "Mobile Home", Price = 300, MaxPersonLimit = 8 },
                new SpotType { Id = 4, Name = "Cabin - Small", Price = 600, MaxPersonLimit = 6 },
                new SpotType { Id = 5, Name = "Cabin - Medium", Price = 800, MaxPersonLimit = 8 },
                new SpotType { Id = 6, Name = "Cabin - Large", Price = 1200, MaxPersonLimit = 12 }
            );

            // CampSites
            modelBuilder.Entity<CampSite>().HasData(
                new CampSite { Id = 1, Name = "Solgläntan", Adress = "Solvägen 1", Description = "Skogsnära camping med badsjö" },
                new CampSite { Id = 2, Name = "Havsutsikten", Adress = "Strandvägen 2", Description = "Havsnära camping med aktiviteter" }
            );

            // CampSpots
            modelBuilder.Entity<CampSpot>().HasData(
                // Tent (TypeId = 1)
                new CampSpot { Id = 1, CampSiteId = 1, TypeId = 1, Electricity = false },
                new CampSpot { Id = 2, CampSiteId = 1, TypeId = 1, Electricity = true },
                new CampSpot { Id = 3, CampSiteId = 2, TypeId = 1, Electricity = false },

                // Caravan (TypeId = 2)
                new CampSpot { Id = 4, CampSiteId = 1, TypeId = 2, Electricity = true },
                new CampSpot { Id = 5, CampSiteId = 2, TypeId = 2, Electricity = false },
                new CampSpot { Id = 6, CampSiteId = 2, TypeId = 2, Electricity = true },

                // Mobile Home (TypeId = 3) – alltid el
                new CampSpot { Id = 7, CampSiteId = 1, TypeId = 3, Electricity = true },
                new CampSpot { Id = 8, CampSiteId = 2, TypeId = 3, Electricity = true },
                new CampSpot { Id = 9, CampSiteId = 2, TypeId = 3, Electricity = true },

                // Cabin - Small (TypeId = 4) – alltid el
                new CampSpot { Id = 10, CampSiteId = 1, TypeId = 4, Electricity = true },
                new CampSpot { Id = 11, CampSiteId = 1, TypeId = 4, Electricity = true },
                new CampSpot { Id = 12, CampSiteId = 2, TypeId = 4, Electricity = true },

                // Cabin - Medium (TypeId = 5) – alltid el
                new CampSpot { Id = 13, CampSiteId = 1, TypeId = 5, Electricity = true },
                new CampSpot { Id = 14, CampSiteId = 2, TypeId = 5, Electricity = true },
                new CampSpot { Id = 15, CampSiteId = 2, TypeId = 5, Electricity = true },

                // Cabin - Large (TypeId = 6) – alltid el
                new CampSpot { Id = 16, CampSiteId = 1, TypeId = 6, Electricity = true },
                new CampSpot { Id = 17, CampSiteId = 1, TypeId = 6, Electricity = true },
                new CampSpot { Id = 18, CampSiteId = 2, TypeId = 6, Electricity = true }
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
                },
                new Customer
                {
                    Id = 3,
                    FirstName = "Carina",
                    LastName = "Carlsson",
                    Email = "carina.carlsson@example.com",
                    PhoneNumber = "0705678910",
                    StreetAddress = "Stugvägen 5",
                    ZipCode = "54321",
                    City = "Falkenberg"
                },
                new Customer
                {
                    Id = 4,
                    FirstName = "David",
                    LastName = "Dahl",
                    Email = "david.dahl@example.com",
                    PhoneNumber = "0723456789",
                    StreetAddress = "Ängsbacken 7",
                    ZipCode = "67890",
                    City = "Laholm"
                },
                new Customer
                {
                    Id = 5,
                    FirstName = "Eva",
                    LastName = "Ekström",
                    Email = "eva.ekstrom@example.com",
                    PhoneNumber = "0761122334",
                    StreetAddress = "Tallgatan 22",
                    ZipCode = "11223",
                    City = "Kungsbacka"
                },
                new Customer
                {
                    Id = 6,
                    FirstName = "Fredrik",
                    LastName = "Friberg",
                    Email = "fredrik.friberg@example.com",
                    PhoneNumber = "0794455667",
                    StreetAddress = "Granvägen 3",
                    ZipCode = "33445",
                    City = "Göteborg"
                }
            );

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
                },
                new Booking
                {
                    Id = 3,
                    CustomerId = 3,
                    CampSpotId = 5,
                    StartDate = new DateTime(2025, 5, 30),
                    EndDate = new DateTime(2025, 6, 2),
                    NumberOfPeople = 2
                },
                new Booking
                {
                    Id = 4,
                    CustomerId = 4,
                    CampSpotId = 6,
                    StartDate = new DateTime(2025, 6, 10),
                    EndDate = new DateTime(2025, 6, 12),
                    NumberOfPeople = 1
                },
                new Booking
                {
                    Id = 5,
                    CustomerId = 5,
                    CampSpotId = 9,
                    StartDate = new DateTime(2025, 7, 15),
                    EndDate = new DateTime(2025, 7, 19),
                    NumberOfPeople = 3
                },
                new Booking
                {
                    Id = 6,
                    CustomerId = 6,
                    CampSpotId = 10,
                    StartDate = new DateTime(2025, 8, 5),
                    EndDate = new DateTime(2025, 8, 10),
                    NumberOfPeople = 2
                },
                new Booking
                {
                    Id = 7,
                    CustomerId = 1,
                    CampSpotId = 12,
                    StartDate = new DateTime(2025, 8, 15),
                    EndDate = new DateTime(2025, 8, 18),
                    NumberOfPeople = 2
                },
                new Booking
                {
                    Id = 8,
                    CustomerId = 2,
                    CampSpotId = 14,
                    StartDate = new DateTime(2025, 9, 2),
                    EndDate = new DateTime(2025, 9, 5),
                    NumberOfPeople = 4
                },
                new Booking
                {
                    Id = 9,
                    CustomerId = 3,
                    CampSpotId = 16,
                    StartDate = new DateTime(2025, 10, 1),
                    EndDate = new DateTime(2025, 10, 4),
                    NumberOfPeople = 5
                },
                new Booking
                {
                    Id = 10,
                    CustomerId = 4,
                    CampSpotId = 18,
                    StartDate = new DateTime(2025, 11, 10),
                    EndDate = new DateTime(2025, 11, 14),
                    NumberOfPeople = 6
                }
            );

        }
    }
}
