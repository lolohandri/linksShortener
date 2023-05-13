using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkAPI.Migrations
{
    /// <inheritdoc />
    public partial class TimeofcreationlinkandcreatedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Users",
                newName: "UserRole");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Links",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Links",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Links");

            migrationBuilder.RenameColumn(
                name: "UserRole",
                table: "Users",
                newName: "Role");
        }
    }
}
