using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionHouse.Migrations
{
    public partial class AddedTypeToVehicleEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Vehicles",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Vehicles");
        }
    }
}
