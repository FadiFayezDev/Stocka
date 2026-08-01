using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSellingPriceInProductMode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE stock_movements ALTER COLUMN reference_type TYPE integer USING reference_type::integer;");

            migrationBuilder.AlterColumn<int>(
                name: "reference_type",
                table: "stock_movements",
                type: "integer",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "selling_price",
                table: "products",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "selling_price",
                table: "products");

            migrationBuilder.AlterColumn<string>(
                name: "reference_type",
                table: "stock_movements",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
