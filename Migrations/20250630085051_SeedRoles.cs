﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                       table: "AspNetRoles",
                       columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                       values: new object[,]
                       {
                { Guid.NewGuid().ToString(), "Admin", "Admin", Guid.NewGuid().ToString() },
                       });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                      table: "AspNetRoles",
                      keyColumn: "Name",
                      keyValue: "Admin");

        }
    }
}
