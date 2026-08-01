using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeBranchAssignmentAndAuthScopes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "branch_id",
                table: "employees",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_employees_branch_id",
                table: "employees",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "ix_employees_brand_id_branch_id",
                table: "employees",
                columns: new[] { "brand_id", "branch_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_employees_branches_branch_id",
                table: "employees",
                column: "branch_id",
                principalTable: "branches",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_employees_branches_branch_id",
                table: "employees");

            migrationBuilder.DropIndex(
                name: "ix_employees_branch_id",
                table: "employees");

            migrationBuilder.DropIndex(
                name: "ix_employees_brand_id_branch_id",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "branch_id",
                table: "employees");
        }
    }
}
