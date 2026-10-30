using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .InsertData(
                    table: "AspNetRoles",
                    columns: new string[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                    values: new object[] { Guid.NewGuid(), "Owner", "Owner".ToUpper(), Guid.NewGuid().ToString() });

            migrationBuilder
                .InsertData(
                    table: "AspNetRoles",
                    columns: new string[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                    values: new object[] { Guid.NewGuid(), "Manager", "Manager".ToUpper(), Guid.NewGuid().ToString() });

            migrationBuilder
                .InsertData(
                    table: "AspNetRoles",
                    columns: new string[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                    values: new object[] { Guid.NewGuid(), "Cashier", "Cashier".ToUpper(), Guid.NewGuid().ToString() });

            migrationBuilder
                .InsertData(
                    table: "AspNetRoles",
                    columns: new string[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                    values: new object[] { Guid.NewGuid(), "WarehouseAssociate", "WarehouseAssociate".ToUpper(), Guid.NewGuid().ToString() });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELECT FROM AspNetRoles");
        }
    }
}