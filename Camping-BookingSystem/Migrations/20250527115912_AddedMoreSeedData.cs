using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingSystem_ClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddedMoreSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxPersonLimit",
                table: "SpotTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 2,
                column: "TypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Electricity", "TypeId" },
                values: new object[] { false, 1 });

            migrationBuilder.InsertData(
                table: "CampSpots",
                columns: new[] { "Id", "CampSiteId", "Electricity", "TypeId" },
                values: new object[,]
                {
                    { 4, 1, true, 2 },
                    { 5, 2, false, 2 },
                    { 6, 2, true, 2 },
                    { 7, 1, true, 3 },
                    { 8, 2, true, 3 },
                    { 9, 2, true, 3 }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "City", "Email", "FirstName", "LastName", "PhoneNumber", "StreetAddress", "ZipCode" },
                values: new object[,]
                {
                    { 3, "Falkenberg", "carina.carlsson@example.com", "Carina", "Carlsson", "0705678910", "Stugvägen 5", "54321" },
                    { 4, "Laholm", "david.dahl@example.com", "David", "Dahl", "0723456789", "Ängsbacken 7", "67890" },
                    { 5, "Kungsbacka", "eva.ekstrom@example.com", "Eva", "Ekström", "0761122334", "Tallgatan 22", "11223" },
                    { 6, "Göteborg", "fredrik.friberg@example.com", "Fredrik", "Friberg", "0794455667", "Granvägen 3", "33445" }
                });

            migrationBuilder.UpdateData(
                table: "SpotTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "MaxPersonLimit",
                value: 8);

            migrationBuilder.UpdateData(
                table: "SpotTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "MaxPersonLimit",
                value: 8);

            migrationBuilder.UpdateData(
                table: "SpotTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "MaxPersonLimit", "Name", "Price" },
                values: new object[] { 8, "Mobile Home", 300m });

            migrationBuilder.InsertData(
                table: "SpotTypes",
                columns: new[] { "Id", "MaxPersonLimit", "Name", "Price" },
                values: new object[,]
                {
                    { 4, 6, "Cabin - Small", 600m },
                    { 5, 8, "Cabin - Medium", 800m },
                    { 6, 12, "Cabin - Large", 1200m }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "CampSpotId", "CustomerId", "EndDate", "NumberOfPeople", "Parking", "StartDate", "Status", "Wifi" },
                values: new object[,]
                {
                    { 3, 5, 3, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, false, new DateTime(2025, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, false },
                    { 4, 6, 4, new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, false },
                    { 5, 9, 5, new DateTime(2025, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, false, new DateTime(2025, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, false }
                });

            migrationBuilder.InsertData(
                table: "CampSpots",
                columns: new[] { "Id", "CampSiteId", "Electricity", "TypeId" },
                values: new object[,]
                {
                    { 10, 1, true, 4 },
                    { 11, 1, true, 4 },
                    { 12, 2, true, 4 },
                    { 13, 1, true, 5 },
                    { 14, 2, true, 5 },
                    { 15, 2, true, 5 },
                    { 16, 1, true, 6 },
                    { 17, 1, true, 6 },
                    { 18, 2, true, 6 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "CampSpotId", "CustomerId", "EndDate", "NumberOfPeople", "Parking", "StartDate", "Status", "Wifi" },
                values: new object[,]
                {
                    { 6, 10, 6, new DateTime(2025, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, false, new DateTime(2025, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, false },
                    { 7, 12, 1, new DateTime(2025, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, false, new DateTime(2025, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, false },
                    { 8, 14, 2, new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, false, new DateTime(2025, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, false },
                    { 9, 16, 3, new DateTime(2025, 10, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, false, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, false },
                    { 10, 18, 4, new DateTime(2025, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, false, new DateTime(2025, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "SpotTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SpotTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "SpotTypes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "MaxPersonLimit",
                table: "SpotTypes");

            migrationBuilder.UpdateData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 2,
                column: "TypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Electricity", "TypeId" },
                values: new object[] { true, 3 });

            migrationBuilder.UpdateData(
                table: "SpotTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Cabin", 500m });
        }
    }
}
