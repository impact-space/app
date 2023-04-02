using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImpactSpace.Core.Migrations
{
    /// <inheritdoc />
    public partial class ProjectTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrganizationMemberActivities_AppActivities_ActivityId",
                table: "AppOrganizationMemberActivities");

            migrationBuilder.RenameColumn(
                name: "ActivityId",
                table: "AppOrganizationMemberActivities",
                newName: "ObjectiveId");

            migrationBuilder.RenameIndex(
                name: "IX_AppOrganizationMemberActivities_ActivityId",
                table: "AppOrganizationMemberActivities",
                newName: "IX_AppOrganizationMemberActivities_ObjectiveId");

            migrationBuilder.CreateTable(
                name: "AppTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppProjectTags",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProjectTags", x => new { x.ProjectId, x.TagId });
                    table.ForeignKey(
                        name: "FK_AppProjectTags_AppProjects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "AppProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppProjectTags_AppTags_TagId",
                        column: x => x.TagId,
                        principalTable: "AppTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppProjectTags_TagId",
                table: "AppProjectTags",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrganizationMemberActivities_AppActivities_ObjectiveId",
                table: "AppOrganizationMemberActivities",
                column: "ObjectiveId",
                principalTable: "AppActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrganizationMemberActivities_AppActivities_ObjectiveId",
                table: "AppOrganizationMemberActivities");

            migrationBuilder.DropTable(
                name: "AppProjectTags");

            migrationBuilder.DropTable(
                name: "AppTags");

            migrationBuilder.RenameColumn(
                name: "ObjectiveId",
                table: "AppOrganizationMemberActivities",
                newName: "ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_AppOrganizationMemberActivities_ObjectiveId",
                table: "AppOrganizationMemberActivities",
                newName: "IX_AppOrganizationMemberActivities_ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrganizationMemberActivities_AppActivities_ActivityId",
                table: "AppOrganizationMemberActivities",
                column: "ActivityId",
                principalTable: "AppActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
