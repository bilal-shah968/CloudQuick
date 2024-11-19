using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CloudQuick.Migrations
{
    /// <inheritdoc />
    public partial class AddDataTOStudentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Address", "DOB", "Email", "StudentName" },
                values: new object[,]
                {
                    { 1, "Pakistan", new DateTime(2024, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shah@gmail.com", "Bilal Shah" },
                    { 2, "Pakistan", new DateTime(2024, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bilal@gmail.com", "Bilal" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
