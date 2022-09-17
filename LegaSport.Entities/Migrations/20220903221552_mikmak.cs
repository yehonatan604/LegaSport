using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LegaSport.Entities.Migrations
{
    public partial class mikmak : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClothType",
                table: "Shorts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClothType",
                table: "Shoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClothType",
                table: "Shirts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClothType",
                table: "Pants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClothType",
                table: "Shorts");

            migrationBuilder.DropColumn(
                name: "ClothType",
                table: "Shoes");

            migrationBuilder.DropColumn(
                name: "ClothType",
                table: "Shirts");

            migrationBuilder.DropColumn(
                name: "ClothType",
                table: "Pants");
        }
    }
}
