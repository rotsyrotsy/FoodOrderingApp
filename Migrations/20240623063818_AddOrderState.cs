using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodOrderingApp.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "CategoryName",
            //    table: "Dish");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Dish",
                type: "decimal(20,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Dish",
                type: "decimal(3,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,2)");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Dish",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
