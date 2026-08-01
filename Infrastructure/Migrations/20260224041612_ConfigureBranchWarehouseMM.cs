using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureBranchWarehouseMM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Branches_BranchId",
                table: "warehouses");

            migrationBuilder.DropIndex(
                name: "ix_warehouses_branch_id",
                table: "warehouses");

            migrationBuilder.DropColumn(
                name: "branch_id",
                table: "warehouses");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "warehouses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "warehouses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "warehouse_branch",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    brand_id = table.Column<Guid>(type: "uuid", nullable: false),
                    branch_id = table.Column<Guid>(type: "uuid", nullable: false),
                    warehouse_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_warehouse_branch", x => x.id);
                    table.ForeignKey(
                        name: "fk_warehouse_branch_branches_branch_id",
                        column: x => x.branch_id,
                        principalTable: "branches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_warehouse_branch_brands_brand_id",
                        column: x => x.brand_id,
                        principalTable: "brands",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_warehouse_branch_warehouses_warehouse_id",
                        column: x => x.warehouse_id,
                        principalTable: "warehouses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_warehouse_branch_branch_id_warehouse_id",
                table: "warehouse_branch",
                columns: new[] { "branch_id", "warehouse_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_warehouse_branch_brand_id",
                table: "warehouse_branch",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "ix_warehouse_branch_warehouse_id",
                table: "warehouse_branch",
                column: "warehouse_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "warehouse_branch");

            migrationBuilder.DropColumn(
                name: "description",
                table: "warehouses");

            migrationBuilder.DropColumn(
                name: "location",
                table: "warehouses");

            migrationBuilder.AddColumn<Guid>(
                name: "branch_id",
                table: "warehouses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_warehouses_branch_id",
                table: "warehouses",
                column: "branch_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Branches_BranchId",
                table: "warehouses",
                column: "branch_id",
                principalTable: "branches",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
