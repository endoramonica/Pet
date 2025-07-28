using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceSystem.Api.Migrations
{
    public partial class AddPetTypeToPet_RemoveAgeinPet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Xoá cột Age (int)
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Pets");

            // Thêm cột enum PetType (kiểu int)
            migrationBuilder.AddColumn<int>(
                name: "PetType",
                table: "Pets",
                type: "int",
                nullable: false,
                defaultValue: 0); // Mặc định là Dog nếu Dog = 0

            // Thêm cột ngày sinh
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Pets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 1, 1));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PetType",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Pets");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Pets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
