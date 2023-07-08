using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDifficultiesAndRegions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("aa374cde-b1a7-44a0-b03a-c683b16556ec"), "Medium" },
                    { new Guid("b7567802-b688-4355-809e-fd344ef9e929"), "Hard" },
                    { new Guid("cede998f-110b-4206-babd-97c82b516642"), "Easy" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("34aaf3a6-e25f-4b9b-aaf8-6b2dc4720456"), "BOP", null, "Bay of Plenty" },
                    { new Guid("3fd2b0b5-3662-4575-923c-07701df2bf82"), "NSN", "https://photos.smugmug.com/New-Zealand-2016/Nelson-/i-wvD5rSk/0/O/Nelson%20New%20Zealand-4693.jpg", "Nelson" },
                    { new Guid("4a5f65d7-1f19-4578-889c-3d44d23e8584"), "STL", null, "Southland" },
                    { new Guid("53ddd3ff-bc43-416a-9036-c8f9aa8470bf"), "AKL", "https://images.pexels.com/photos/5342974/pexels-photo-5342974.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2", "Auckland" },
                    { new Guid("d40c77a5-8939-4198-9023-dc4929a6f799"), "WGN", "https://images.pexels.com/photos/8379417/pexels-photo-8379417.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2", "Wellington" },
                    { new Guid("e8380a74-7558-4074-aa9d-813cdfe440bc"), "NTL", null, "Northland" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("aa374cde-b1a7-44a0-b03a-c683b16556ec"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("b7567802-b688-4355-809e-fd344ef9e929"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("cede998f-110b-4206-babd-97c82b516642"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("34aaf3a6-e25f-4b9b-aaf8-6b2dc4720456"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("3fd2b0b5-3662-4575-923c-07701df2bf82"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("4a5f65d7-1f19-4578-889c-3d44d23e8584"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("53ddd3ff-bc43-416a-9036-c8f9aa8470bf"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("d40c77a5-8939-4198-9023-dc4929a6f799"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("e8380a74-7558-4074-aa9d-813cdfe440bc"));
        }
    }
}
