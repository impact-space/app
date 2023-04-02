using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImpactSpace.Core.Migrations
{
    /// <inheritdoc />
    public partial class RenamedEntities1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppActivities_AppQuests_QuestId",
                table: "AppActivities");

            migrationBuilder.DropTable(
                name: "AppOrganizationMemberActivities");

            migrationBuilder.DropTable(
                name: "AppQuests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppActivities",
                table: "AppActivities");

            migrationBuilder.DropColumn(
                name: "EstimatedEffort",
                table: "AppActivities");

            migrationBuilder.DropColumn(
                name: "PriorityLevel",
                table: "AppActivities");

            migrationBuilder.RenameTable(
                name: "AppActivities",
                newName: "AppObjectives");

            migrationBuilder.RenameColumn(
                name: "QuestId",
                table: "AppObjectives",
                newName: "MilestoneId");

            migrationBuilder.RenameIndex(
                name: "IX_AppActivities_QuestId",
                table: "AppObjectives",
                newName: "IX_AppObjectives_MilestoneId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppObjectives",
                table: "AppObjectives",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    StatusType = table.Column<int>(type: "integer", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PriorityLevel = table.Column<int>(type: "integer", nullable: false),
                    ObjectiveId = table.Column<Guid>(type: "uuid", nullable: false),
                    Budget = table.Column<decimal>(type: "numeric", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EstimatedEffort = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_AppActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppActions_AppObjectives_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalTable: "AppObjectives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppOrganizationMemberActions",
                columns: table => new
                {
                    OrganizationMemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActionId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppOrganizationMemberActions", x => new { x.OrganizationMemberId, x.ActionId });
                    table.ForeignKey(
                        name: "FK_AppOrganizationMemberActions_AppActions_ActionId",
                        column: x => x.ActionId,
                        principalTable: "AppActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppOrganizationMemberActions_AppOrganizationMembers_Organiz~",
                        column: x => x.OrganizationMemberId,
                        principalTable: "AppOrganizationMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppActions_ObjectiveId",
                table: "AppActions",
                column: "ObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOrganizationMemberActions_ActionId",
                table: "AppOrganizationMemberActions",
                column: "ActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppObjectives_AppMilestones_MilestoneId",
                table: "AppObjectives",
                column: "MilestoneId",
                principalTable: "AppMilestones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppObjectives_AppMilestones_MilestoneId",
                table: "AppObjectives");

            migrationBuilder.DropTable(
                name: "AppOrganizationMemberActions");

            migrationBuilder.DropTable(
                name: "AppActions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppObjectives",
                table: "AppObjectives");

            migrationBuilder.RenameTable(
                name: "AppObjectives",
                newName: "AppActivities");

            migrationBuilder.RenameColumn(
                name: "MilestoneId",
                table: "AppActivities",
                newName: "QuestId");

            migrationBuilder.RenameIndex(
                name: "IX_AppObjectives_MilestoneId",
                table: "AppActivities",
                newName: "IX_AppActivities_QuestId");

            migrationBuilder.AddColumn<int>(
                name: "EstimatedEffort",
                table: "AppActivities",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PriorityLevel",
                table: "AppActivities",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppActivities",
                table: "AppActivities",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppOrganizationMemberActivities",
                columns: table => new
                {
                    OrganizationMemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    ObjectiveId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppOrganizationMemberActivities", x => new { x.OrganizationMemberId, x.ObjectiveId });
                    table.ForeignKey(
                        name: "FK_AppOrganizationMemberActivities_AppActivities_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalTable: "AppActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppOrganizationMemberActivities_AppOrganizationMembers_Orga~",
                        column: x => x.OrganizationMemberId,
                        principalTable: "AppOrganizationMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppQuests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MilestoneId = table.Column<Guid>(type: "uuid", nullable: false),
                    Budget = table.Column<decimal>(type: "numeric", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    StatusType = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppQuests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppQuests_AppMilestones_MilestoneId",
                        column: x => x.MilestoneId,
                        principalTable: "AppMilestones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppOrganizationMemberActivities_ObjectiveId",
                table: "AppOrganizationMemberActivities",
                column: "ObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_AppQuests_MilestoneId",
                table: "AppQuests",
                column: "MilestoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppActivities_AppQuests_QuestId",
                table: "AppActivities",
                column: "QuestId",
                principalTable: "AppQuests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
