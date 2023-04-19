using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImpactSpace.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPhoneNumberObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber_CountryCode",
                table: "AppOrganizationProfiles");

            migrationBuilder.DropColumn(
                name: "PhoneNumber_CountryCode",
                table: "AppOrganizationMembers");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber_NationalNumber",
                table: "AppOrganizationProfiles",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber_NationalNumber",
                table: "AppOrganizationMembers",
                newName: "PhoneNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "AppOrganizationProfiles",
                newName: "PhoneNumber_NationalNumber");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "AppOrganizationMembers",
                newName: "PhoneNumber_NationalNumber");

            migrationBuilder.AddColumn<int>(
                name: "PhoneNumber_CountryCode",
                table: "AppOrganizationProfiles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhoneNumber_CountryCode",
                table: "AppOrganizationMembers",
                type: "integer",
                nullable: true);
        }
    }
}
