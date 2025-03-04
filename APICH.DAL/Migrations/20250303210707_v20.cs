using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICH.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_User_UserNumber",
                table: "Reviews");

            migrationBuilder.AlterColumn<string>(
                name: "UserNumber",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_User_UserNumber",
                table: "Reviews",
                column: "UserNumber",
                principalTable: "User",
                principalColumn: "Number",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_User_UserNumber",
                table: "Reviews");

            migrationBuilder.AlterColumn<string>(
                name: "UserNumber",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_User_UserNumber",
                table: "Reviews",
                column: "UserNumber",
                principalTable: "User",
                principalColumn: "Number");
        }
    }
}
