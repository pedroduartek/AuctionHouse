﻿// <auto-generated />
using System;
using AuctionHouse.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AuctionHouse.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250308104055_Changed-Vehicle-Auction-Relationship")]
    partial class ChangedVehicleAuctionRelationship
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("AuctionHouse.Models.Entities.AuctionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<double?>("CurrentBid")
                        .HasColumnType("REAL");

                    b.Property<string>("CurrentBidder")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAuctionActive")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("VehcileId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("VehcileId");

                    b.ToTable("Auctions");
                });

            modelBuilder.Entity("AuctionHouse.Models.Entities.VehicleEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<double>("LoadCapacity")
                        .HasColumnType("REAL");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberOfDoors")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfSeats")
                        .HasColumnType("INTEGER");

                    b.Property<double>("StartingBid")
                        .HasColumnType("REAL");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("AuctionHouse.Models.Entities.AuctionEntity", b =>
                {
                    b.HasOne("AuctionHouse.Models.Entities.VehicleEntity", "Vehcile")
                        .WithMany()
                        .HasForeignKey("VehcileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vehcile");
                });
#pragma warning restore 612, 618
        }
    }
}
