using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingAPI.Migrations
{
    /// <inheritdoc />
    public partial class Alimentartabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateUpdated",
                table: "Bookings",
                newName: "DateModified");

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "BedsOcuppied", "DateCreated", "DateModified", "Detail", "Fee", "Name" },
                values: new object[,]
                {
                    { 3, 3, new DateTime(2024, 3, 1, 16, 37, 44, 493, DateTimeKind.Local).AddTicks(6394), new DateTime(2024, 3, 1, 16, 37, 44, 493, DateTimeKind.Local).AddTicks(6412), "Abonó 10%", 200.0, "Ricardo" },
                    { 4, 4, new DateTime(2024, 3, 1, 16, 37, 44, 493, DateTimeKind.Local).AddTicks(6416), new DateTime(2024, 3, 1, 16, 37, 44, 493, DateTimeKind.Local).AddTicks(6417), "Abonó 10%", 150.0, "Romualdo" }
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

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "Bookings",
                newName: "DateUpdated");
        }
    }
}
