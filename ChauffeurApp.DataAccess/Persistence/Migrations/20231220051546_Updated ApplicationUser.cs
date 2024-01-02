using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChauffeurApp.DataAccess.Migrations
{
    public partial class UpdatedApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePhoto",
                table: "AspNetUsers",
                newName: "ProfilePicturePath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePicturePath",
                table: "AspNetUsers",
                newName: "ProfilePhoto");
        }
    }
}
