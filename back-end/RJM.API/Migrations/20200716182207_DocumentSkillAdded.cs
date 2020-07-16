using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RJM.API.Migrations
{
    public partial class DocumentSkillAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentSkill",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DocumentId = table.Column<Guid>(nullable: false),
                    SkillId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<Guid>(nullable: false),
                    ModifiedByUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentSkill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentSkill_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentSkill_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentSkill_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentSkill_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSkill_CreatedByUserId",
                table: "DocumentSkill",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSkill_DocumentId",
                table: "DocumentSkill",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSkill_ModifiedByUserId",
                table: "DocumentSkill",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSkill_SkillId",
                table: "DocumentSkill",
                column: "SkillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentSkill");
        }
    }
}
