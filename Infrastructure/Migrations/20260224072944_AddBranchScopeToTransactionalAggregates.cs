using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBranchScopeToTransactionalAggregates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_purchases_brand_id",
                table: "purchases");

            migrationBuilder.DropIndex(
                name: "ix_orders_brand_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "ix_expenses_brand_id",
                table: "expenses");

            migrationBuilder.AddColumn<Guid>(
                name: "branch_id",
                table: "purchases",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "branch_id",
                table: "orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "branch_id",
                table: "expenses",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_purchases_branch_id",
                table: "purchases",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "ix_purchases_brand_id_branch_id_purchase_date",
                table: "purchases",
                columns: new[] { "brand_id", "branch_id", "purchase_date" });

            migrationBuilder.CreateIndex(
                name: "ix_orders_branch_id",
                table: "orders",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_brand_id_branch_id_order_date",
                table: "orders",
                columns: new[] { "brand_id", "branch_id", "order_date" });

            migrationBuilder.CreateIndex(
                name: "ix_expenses_branch_id",
                table: "expenses",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "ix_expenses_brand_id_branch_id_expense_date",
                table: "expenses",
                columns: new[] { "brand_id", "branch_id", "expense_date" });

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Branches_BranchId",
                table: "expenses",
                column: "branch_id",
                principalTable: "branches",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Branches_BranchId",
                table: "orders",
                column: "branch_id",
                principalTable: "branches",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Branches_BranchId",
                table: "purchases",
                column: "branch_id",
                principalTable: "branches",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Branches_BranchId",
                table: "expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Branches_BranchId",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Branches_BranchId",
                table: "purchases");

            migrationBuilder.DropIndex(
                name: "ix_purchases_branch_id",
                table: "purchases");

            migrationBuilder.DropIndex(
                name: "ix_purchases_brand_id_branch_id_purchase_date",
                table: "purchases");

            migrationBuilder.DropIndex(
                name: "ix_orders_branch_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "ix_orders_brand_id_branch_id_order_date",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "ix_expenses_branch_id",
                table: "expenses");

            migrationBuilder.DropIndex(
                name: "ix_expenses_brand_id_branch_id_expense_date",
                table: "expenses");

            migrationBuilder.DropColumn(
                name: "branch_id",
                table: "purchases");

            migrationBuilder.DropColumn(
                name: "branch_id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "branch_id",
                table: "expenses");

            migrationBuilder.CreateIndex(
                name: "ix_purchases_brand_id",
                table: "purchases",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_brand_id",
                table: "orders",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "ix_expenses_brand_id",
                table: "expenses",
                column: "brand_id");
        }
    }
}
