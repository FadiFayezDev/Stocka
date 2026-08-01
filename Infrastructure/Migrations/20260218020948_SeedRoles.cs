using Domain.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string[] roleNames = Enum.GetNames(typeof(SystemRolesType));

            var roles = new object[roleNames.Length, 4];

            for (int i = 0; i < roleNames.Length; i++)
            {
                roles[i, 0] = Guid.NewGuid().ToString();
                roles[i, 1] = Guid.NewGuid().ToString();
                roles[i, 2] = roleNames[i];
                roles[i, 3] = roleNames[i].ToUpper();
            }

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: roles
             );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM \"asp_net_roles\"");
        }
    }
}