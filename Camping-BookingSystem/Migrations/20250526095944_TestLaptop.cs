using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSystem_ClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class TestLaptop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampSpots_SpotTypes_SpotTypeId",
                table: "CampSpots");

            migrationBuilder.DropIndex(
                name: "IX_CampSpots_SpotTypeId",
                table: "CampSpots");

            migrationBuilder.DropColumn(
                name: "SpotTypeId",
                table: "CampSpots");

            migrationBuilder.CreateIndex(
                name: "IX_CampSpots_TypeId",
                table: "CampSpots",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampSpots_SpotTypes_TypeId",
                table: "CampSpots",
                column: "TypeId",
                principalTable: "SpotTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampSpots_SpotTypes_TypeId",
                table: "CampSpots");

            migrationBuilder.DropIndex(
                name: "IX_CampSpots_TypeId",
                table: "CampSpots");

            migrationBuilder.AddColumn<int>(
                name: "SpotTypeId",
                table: "CampSpots",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 1,
                column: "SpotTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 2,
                column: "SpotTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "CampSpots",
                keyColumn: "Id",
                keyValue: 3,
                column: "SpotTypeId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_CampSpots_SpotTypeId",
                table: "CampSpots",
                column: "SpotTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampSpots_SpotTypes_SpotTypeId",
                table: "CampSpots",
                column: "SpotTypeId",
                principalTable: "SpotTypes",
                principalColumn: "Id");
        }
    }
}
