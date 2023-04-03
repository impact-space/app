using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImpactSpace.Core.Migrations
{
    /// <inheritdoc />
    public partial class Challenges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppChallenges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppChallenges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppOrganizationMemberChallenges",
                columns: table => new
                {
                    OrganizationMemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChallengeId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppOrganizationMemberChallenges", x => new { x.OrganizationMemberId, x.ChallengeId });
                    table.ForeignKey(
                        name: "FK_AppOrganizationMemberChallenges_AppChallenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "AppChallenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppOrganizationMemberChallenges_AppOrganizationMembers_Orga~",
                        column: x => x.OrganizationMemberId,
                        principalTable: "AppOrganizationMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppProjectChallenges",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChallengeId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProjectChallenges", x => new { x.ProjectId, x.ChallengeId });
                    table.ForeignKey(
                        name: "FK_AppProjectChallenges_AppChallenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "AppChallenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppProjectChallenges_AppProjects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "AppProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppOrganizationMemberChallenges_ChallengeId",
                table: "AppOrganizationMemberChallenges",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProjectChallenges_ChallengeId",
                table: "AppProjectChallenges",
                column: "ChallengeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppOrganizationMemberChallenges");

            migrationBuilder.DropTable(
                name: "AppProjectChallenges");

            migrationBuilder.DropTable(
                name: "AppChallenges");
        }
    }
}
