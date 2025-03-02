using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbeerRestaurant.Migrations.AbeerRestaurant
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoodItem",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Item_name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Item_desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false),
                    Vegetarian = table.Column<bool>(type: "bit", nullable: true),
                    Price = table.Column<decimal>(type: "Money", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodItem", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Items = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "Money", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodItem");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
