using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionHouse.Migrations
{
    /// <inheritdoc />
    public partial class ChangedVehicleAuctionRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_AuctionInfo_AuctionInfoId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "AuctionInfo");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_AuctionInfoId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "AuctionInfoId",
                table: "Vehicles");

            migrationBuilder.CreateTable(
                name: "Auctions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsAuctionActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CurrentBid = table.Column<double>(type: "REAL", nullable: true),
                    CurrentBidder = table.Column<string>(type: "TEXT", nullable: true),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    VehcileId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auctions_Vehicles_VehcileId",
                        column: x => x.VehcileId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_VehcileId",
                table: "Auctions",
                column: "VehcileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auctions");

            migrationBuilder.AddColumn<Guid>(
                name: "AuctionInfoId",
                table: "Vehicles",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AuctionInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CurrentBid = table.Column<double>(type: "REAL", nullable: true),
                    CurrentBidder = table.Column<string>(type: "TEXT", nullable: true),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsAuctionActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionInfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_AuctionInfoId",
                table: "Vehicles",
                column: "AuctionInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_AuctionInfo_AuctionInfoId",
                table: "Vehicles",
                column: "AuctionInfoId",
                principalTable: "AuctionInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
