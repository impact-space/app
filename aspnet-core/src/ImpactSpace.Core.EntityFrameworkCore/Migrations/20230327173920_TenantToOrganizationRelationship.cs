using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImpactSpace.Core.Migrations
{
    /// <inheritdoc />
    public partial class TenantToOrganizationRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppOrganizations_TenantId",
                table: "AppOrganizations",
                column: "TenantId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrganizations_AbpTenants_TenantId",
                table: "AppOrganizations",
                column: "TenantId",
                principalTable: "AbpTenants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrganizations_AbpTenants_TenantId",
                table: "AppOrganizations");

            migrationBuilder.DropIndex(
                name: "IX_AppOrganizations_TenantId",
                table: "AppOrganizations");
        }
    }
}
