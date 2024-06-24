using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodOrderingApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDishTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
            name: "isAvailable",
            table: "Dish",
            nullable: false,
            defaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
            name: "DateCreation",
            table: "Dish",
            nullable: false,
            defaultValueSql: "GETDATE()",
            oldClrType: typeof(DateTime),
            oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdate",
                table: "Dish",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldNullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "isAvailable",
            table: "Dish");

            migrationBuilder.AlterColumn<DateTime>(
            name: "DateCreation",
            table: "Dish",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdate",
                table: "Dish",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "GETDATE()");
        }
    }
}
