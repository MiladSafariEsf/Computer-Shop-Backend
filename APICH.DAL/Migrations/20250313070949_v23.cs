using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICH.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BanerImage",
                table: "Baners",
                newName: "BanerImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BanerImageUrl",
                table: "Baners",
                newName: "BanerImage");
        }
    }
}
