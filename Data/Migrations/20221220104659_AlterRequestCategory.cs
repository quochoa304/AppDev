using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppDev.Data.Migrations
{
    public partial class AlterRequestCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAppproved",
                table: "RequestCategories",
                newName: "IsApproved");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsApproved",
                table: "RequestCategories",
                newName: "IsAppproved");
        }
    }
}
