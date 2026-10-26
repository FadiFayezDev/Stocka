using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToWarehouseBatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WarehouseBatches_BatchId",
                schema: "Inventory",
                table: "WarehouseBatches");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseBatches_BatchId_WarehouseId",
                schema: "Inventory",
                table: "WarehouseBatches",
                columns: new[] { "BatchId", "WarehouseId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WarehouseBatches_BatchId_WarehouseId",
                schema: "Inventory",
                table: "WarehouseBatches");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseBatches_BatchId",
                schema: "Inventory",
                table: "WarehouseBatches",
                column: "BatchId");
        }
    }
}
