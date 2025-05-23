using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcsess.Migrations
{
    public partial class newColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isUsed",
                table: "CouponUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isUsed",
                table: "CouponProducts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isUsed",
                table: "CouponUsers");

            migrationBuilder.DropColumn(
                name: "isUsed",
                table: "CouponProducts");
        }
    }
}
