using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LegaSport.Entities.Migrations
{
    public partial class newmigu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Users_SalesManId",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "SalesMen");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Users",
                newName: "HireDate");

            migrationBuilder.RenameColumn(
                name: "SalesManId",
                table: "Sales",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Sales_SalesManId",
                table: "Sales",
                newName: "IX_Sales_UserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSale",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalesCount",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SalesTotal",
                table: "Users",
                type: "float",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Users_UserId",
                table: "Sales",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Users_UserId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "LastSale",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SalesCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SalesTotal",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "HireDate",
                table: "Users",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Sales",
                newName: "SalesManId");

            migrationBuilder.RenameIndex(
                name: "IX_Sales_UserId",
                table: "Sales",
                newName: "IX_Sales_SalesManId");

            migrationBuilder.CreateTable(
                name: "SalesMen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastSale = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SalesBalance = table.Column<int>(type: "int", nullable: false),
                    SalesCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesMen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesMen_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesMen_UserId",
                table: "SalesMen",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Users_SalesManId",
                table: "Sales",
                column: "SalesManId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
