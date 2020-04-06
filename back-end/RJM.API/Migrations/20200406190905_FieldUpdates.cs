using Microsoft.EntityFrameworkCore.Migrations;

namespace RJM.API.Migrations
{
    public partial class FieldUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "SizeInBytes",
                table: "Documents",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Primary",
                table: "DocumentResume",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Primary",
                table: "DocumentResume");

            migrationBuilder.AlterColumn<int>(
                name: "SizeInBytes",
                table: "Documents",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
