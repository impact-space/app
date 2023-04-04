using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ImpactSpace.Core.Migrations
{
    /// <inheritdoc />
    public partial class OrganizationEntityProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrganizationMembers_AppProjects_ProjectId",
                table: "AppOrganizationMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProjects_AppOrganizationMembers_ProjectOwnerId",
                table: "AppProjects");

            migrationBuilder.DropIndex(
                name: "IX_AppOrganizationMembers_ProjectId",
                table: "AppOrganizationMembers");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "AppOrganizationMembers");

            migrationBuilder.RenameColumn(
                name: "ProjectOwnerId",
                table: "AppProjects",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_AppProjects_ProjectOwnerId",
                table: "AppProjects",
                newName: "IX_AppProjects_OwnerId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AppOrganizationMembers",
                newName: "IdentityUserId");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "AppOrganizationMembers",
                newName: "PhoneNumber_NationalNumber");

            migrationBuilder.AddColumn<int>(
                name: "PhoneNumber_CountryCode",
                table: "AppOrganizationMembers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppOrganizationMemberProjects",
                columns: table => new
                {
                    OrganizationMemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppOrganizationMemberProjects", x => new { x.OrganizationMemberId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_AppOrganizationMemberProjects_AppOrganizationMembers_Organi~",
                        column: x => x.OrganizationMemberId,
                        principalTable: "AppOrganizationMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppOrganizationMemberProjects_AppProjects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "AppProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppOrganizationMemberSocialMediaLinks",
                columns: table => new
                {
                    OrganizationMemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Platform = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppOrganizationMemberSocialMediaLinks", x => new { x.OrganizationMemberId, x.Id });
                    table.ForeignKey(
                        name: "FK_AppOrganizationMemberSocialMediaLinks_AppOrganizationMember~",
                        column: x => x.OrganizationMemberId,
                        principalTable: "AppOrganizationMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppOrganizationProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    MissionStatement = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Website = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    PhoneNumberCountryCode = table.Column<int>(name: "PhoneNumber_CountryCode", type: "integer", nullable: true),
                    PhoneNumberNationalNumber = table.Column<string>(name: "PhoneNumber_NationalNumber", type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LogoUrl = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppOrganizationProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppOrganizationProfiles_AppOrganizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "AppOrganizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppOrganizationProfileSocialMediaLinks",
                columns: table => new
                {
                    OrganizationProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Platform = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppOrganizationProfileSocialMediaLinks", x => new { x.OrganizationProfileId, x.Id });
                    table.ForeignKey(
                        name: "FK_AppOrganizationProfileSocialMediaLinks_AppOrganizationProfi~",
                        column: x => x.OrganizationProfileId,
                        principalTable: "AppOrganizationProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppOrganizationMembers_IdentityUserId",
                table: "AppOrganizationMembers",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOrganizationMemberProjects_ProjectId",
                table: "AppOrganizationMemberProjects",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOrganizationProfiles_OrganizationId",
                table: "AppOrganizationProfiles",
                column: "OrganizationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrganizationMembers_AbpUsers_IdentityUserId",
                table: "AppOrganizationMembers",
                column: "IdentityUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppProjects_AppOrganizationMembers_OwnerId",
                table: "AppProjects",
                column: "OwnerId",
                principalTable: "AppOrganizationMembers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrganizationMembers_AbpUsers_IdentityUserId",
                table: "AppOrganizationMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProjects_AppOrganizationMembers_OwnerId",
                table: "AppProjects");

            migrationBuilder.DropTable(
                name: "AppOrganizationMemberProjects");

            migrationBuilder.DropTable(
                name: "AppOrganizationMemberSocialMediaLinks");

            migrationBuilder.DropTable(
                name: "AppOrganizationProfileSocialMediaLinks");

            migrationBuilder.DropTable(
                name: "AppOrganizationProfiles");

            migrationBuilder.DropIndex(
                name: "IX_AppOrganizationMembers_IdentityUserId",
                table: "AppOrganizationMembers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber_CountryCode",
                table: "AppOrganizationMembers");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "AppProjects",
                newName: "ProjectOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_AppProjects_OwnerId",
                table: "AppProjects",
                newName: "IX_AppProjects_ProjectOwnerId");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber_NationalNumber",
                table: "AppOrganizationMembers",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "IdentityUserId",
                table: "AppOrganizationMembers",
                newName: "UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "AppOrganizationMembers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppOrganizationMembers_ProjectId",
                table: "AppOrganizationMembers",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrganizationMembers_AppProjects_ProjectId",
                table: "AppOrganizationMembers",
                column: "ProjectId",
                principalTable: "AppProjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppProjects_AppOrganizationMembers_ProjectOwnerId",
                table: "AppProjects",
                column: "ProjectOwnerId",
                principalTable: "AppOrganizationMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
