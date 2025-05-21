using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSystem_ClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingCampSpotTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxPersonLimit",
                table: "CampSpots");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxPersonLimit",
                table: "CampSpots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 1,
                column: "MaxPersonLimit",
                value: 4);

            migrationBuilder.UpdateData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 2,
                column: "MaxPersonLimit",
                value: 6);

            migrationBuilder.UpdateData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 3,
                column: "MaxPersonLimit",
                value: 5);
        }
    }
}
