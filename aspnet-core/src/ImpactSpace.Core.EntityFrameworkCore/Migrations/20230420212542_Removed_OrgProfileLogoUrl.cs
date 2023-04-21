using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImpactSpace.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemovedOrgProfileLogoUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "AppOrganizationProfiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "AppOrganizationProfiles",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true);
        }
    }
}
