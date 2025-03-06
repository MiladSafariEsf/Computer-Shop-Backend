using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICH.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Baners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BanerImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BanerName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baners", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Baners");
        }
    }
}
