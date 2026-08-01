using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBrandIdFromOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_customers_brands_brand_id",
                table: "customers");

            migrationBuilder.DropForeignKey(
                name: "fk_order_items_brands_brand_id",
                table: "order_items");

            migrationBuilder.DropIndex(
                name: "ix_order_items_brand_id",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "brand_id",
                table: "order_items");

            migrationBuilder.AlterColumn<Guid>(
                name: "branch_id",
                table: "employees",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "brand_id",
                table: "customers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_customers_brands_brand_id",
                table: "customers",
                column: "brand_id",
                principalTable: "brands",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_customers_brands_brand_id",
                table: "customers");

            migrationBuilder.AddColumn<Guid>(
                name: "brand_id",
                table: "order_items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "branch_id",
                table: "employees",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "brand_id",
                table: "customers",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "ix_order_items_brand_id",
                table: "order_items",
                column: "brand_id");

            migrationBuilder.AddForeignKey(
                name: "fk_customers_brands_brand_id",
                table: "customers",
                column: "brand_id",
                principalTable: "brands",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_order_items_brands_brand_id",
                table: "order_items",
                column: "brand_id",
                principalTable: "brands",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
