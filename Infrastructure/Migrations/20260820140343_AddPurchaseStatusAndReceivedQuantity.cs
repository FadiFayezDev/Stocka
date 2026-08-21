using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseStatusAndReceivedQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "purchases",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Ordered");

            migrationBuilder.AddColumn<int>(
                name: "received_quantity",
                table: "purchase_items",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "purchases");

            migrationBuilder.DropColumn(
                name: "received_quantity",
                table: "purchase_items");
        }
    }
}
