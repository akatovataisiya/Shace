using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shace.Migrations
{
    public partial class SubsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscribtions",
                table: "Subscribtions");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Subscribtions",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscribtions",
                table: "Subscribtions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Subscribtions_AccountId",
                table: "Subscribtions",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscribtions",
                table: "Subscribtions");

            migrationBuilder.DropIndex(
                name: "IX_Subscribtions_AccountId",
                table: "Subscribtions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Subscribtions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscribtions",
                table: "Subscribtions",
                column: "AccountId");
        }
    }
}
