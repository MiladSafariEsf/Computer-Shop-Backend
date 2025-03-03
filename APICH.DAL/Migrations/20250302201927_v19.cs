using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICH.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "User",
                newName: "Salt");

            migrationBuilder.AddColumn<string>(
                name: "HashedPassword",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashedPassword",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "Salt",
                table: "User",
                newName: "Password");
        }
    }
}
