using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stargate.Services.Migrations
{
    /// <inheritdoc />
    public partial class astronautUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Astronaut");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Astronaut");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Astronaut");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Astronaut");

            migrationBuilder.AlterColumn<int>(
                name: "Title",
                table: "Duty",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Rank",
                table: "Duty",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Duty",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Rank",
                table: "Duty",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Astronaut",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "Astronaut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Astronaut",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Title",
                table: "Astronaut",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
