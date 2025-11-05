using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientsService.Src.Data.Migrations
{
    /// <inheritdoc />
    public partial class FullNameClientUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Clients",
                newName: "FullName");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "BirthDate",
                table: "Clients",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Clients",
                newName: "Name");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Clients",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Clients",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
