using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICH.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCart_User_UserName",
                table: "ShoppingCart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "ShoppingCart",
                newName: "UserNumber");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCart_UserName",
                table: "ShoppingCart",
                newName: "IX_ShoppingCart_UserNumber");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "User",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Number");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCart_User_UserNumber",
                table: "ShoppingCart",
                column: "UserNumber",
                principalTable: "User",
                principalColumn: "Number",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCart_User_UserNumber",
                table: "ShoppingCart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "UserNumber",
                table: "ShoppingCart",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCart_UserNumber",
                table: "ShoppingCart",
                newName: "IX_ShoppingCart_UserName");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "User",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCart_User_UserName",
                table: "ShoppingCart",
                column: "UserName",
                principalTable: "User",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
