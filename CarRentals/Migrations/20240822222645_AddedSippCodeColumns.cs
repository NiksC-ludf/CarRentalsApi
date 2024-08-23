using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentals.Migrations
{
    /// <inheritdoc />
    public partial class AddedSippCodeColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<char>(
                name: "CarBodyType",
                table: "CarOffers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<char>(
                name: "CarCategory",
                table: "CarOffers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<char>(
                name: "CarDriveType",
                table: "CarOffers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<char>(
                name: "CarFuelAndAirConSystem",
                table: "CarOffers",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarBodyType",
                table: "CarOffers");

            migrationBuilder.DropColumn(
                name: "CarCategory",
                table: "CarOffers");

            migrationBuilder.DropColumn(
                name: "CarDriveType",
                table: "CarOffers");

            migrationBuilder.DropColumn(
                name: "CarFuelAndAirConSystem",
                table: "CarOffers");
        }
    }
}
