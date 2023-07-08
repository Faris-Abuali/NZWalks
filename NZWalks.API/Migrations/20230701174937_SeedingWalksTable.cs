using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NZWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingWalksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Walks",
                columns: new[] { "Id", "Description", "DifficultyId", "ImageUrl", "LengthInKm", "Name", "RegionId" },
                values: new object[] { new Guid("38cf7293-a3f3-42cb-8b7d-08db79c1cb12"), "This is a description", new Guid("aa374cde-b1a7-44a0-b03a-c683b16556ec"), "image.jpeg", 8.8300000000000001, "Mount Victoria Lookout Walk", new Guid("53ddd3ff-bc43-416a-9036-c8f9aa8470bf") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Walks",
                keyColumn: "Id",
                keyValue: new Guid("38cf7293-a3f3-42cb-8b7d-08db79c1cb12"));
        }
    }
}
