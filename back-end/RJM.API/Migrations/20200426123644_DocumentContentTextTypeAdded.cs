using Microsoft.EntityFrameworkCore.Migrations;

namespace RJM.API.Migrations
{
    public partial class DocumentContentTextTypeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TextType",
                table: "DocumentContents",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TextType",
                table: "DocumentContents");
        }
    }
}
