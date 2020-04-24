using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace RJM.API.Migrations
{
    public partial class DocumentResumeLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentResume",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DocumentId = table.Column<Guid>(nullable: false),
                    ResumeId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<Guid>(nullable: false),
                    ModifiedByUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentResume", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentResume_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentResume_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentResume_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentResume_Resumes_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentResume_CreatedByUserId",
                table: "DocumentResume",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentResume_DocumentId",
                table: "DocumentResume",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentResume_ModifiedByUserId",
                table: "DocumentResume",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentResume_ResumeId",
                table: "DocumentResume",
                column: "ResumeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentResume");
        }
    }
}
