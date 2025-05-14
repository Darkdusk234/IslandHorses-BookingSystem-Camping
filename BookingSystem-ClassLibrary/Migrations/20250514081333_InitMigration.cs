using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingSystem_ClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CampSites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampSites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreetAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    City = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpotTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpotTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CampSpots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampSiteId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Electricity = table.Column<bool>(type: "bit", nullable: false),
                    MaxPersonLimit = table.Column<int>(type: "int", nullable: false),
                    SpotTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampSpots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampSpots_CampSites_CampSiteId",
                        column: x => x.CampSiteId,
                        principalTable: "CampSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampSpots_SpotTypes_SpotTypeId",
                        column: x => x.SpotTypeId,
                        principalTable: "SpotTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CampSpotId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfPeople = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_CampSpots_CampSpotId",
                        column: x => x.CampSpotId,
                        principalTable: "CampSpots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CampSites",
                columns: new[] { "Id", "Adress", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Solvägen 1", "Skogsnära camping med badsjö", "Solgläntan" },
                    { 2, "Strandvägen 2", "Havsnära camping med aktiviteter", "Havsutsikten" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "City", "Email", "FirstName", "LastName", "PhoneNumber", "StreetAddress", "ZipCode" },
                values: new object[,]
                {
                    { 1, "Varberg", "anna.andersson@example.com", "Anna", "Andersson", "0701234567", "Sommargatan 12", "43245" },
                    { 2, "Halmstad", "bjorn.bergstrom@example.com", "Björn", "Bergström", "0739876543", "Campingvägen 8", "12345" }
                });

            migrationBuilder.InsertData(
                table: "SpotTypes",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Tent", 150m },
                    { 2, "Caravan", 250m },
                    { 3, "Cabin", 500m }
                });

            migrationBuilder.InsertData(
                table: "CampSpots",
                columns: new[] { "Id", "CampSiteId", "Electricity", "MaxPersonLimit", "SpotTypeId", "TypeId" },
                values: new object[,]
                {
                    { 1, 1, false, 4, null, 1 },
                    { 2, 1, true, 6, null, 2 },
                    { 3, 2, true, 5, null, 3 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "CampSpotId", "CustomerId", "EndDate", "NumberOfPeople", "StartDate" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2025, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 3, 2, new DateTime(2025, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CampSpotId",
                table: "Bookings",
                column: "CampSpotId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CustomerId",
                table: "Bookings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CampSpots_CampSiteId",
                table: "CampSpots",
                column: "CampSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_CampSpots_SpotTypeId",
                table: "CampSpots",
                column: "SpotTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "CampSpots");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "CampSites");

            migrationBuilder.DropTable(
                name: "SpotTypes");
        }
    }
}
