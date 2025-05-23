using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcsess.Migrations
{
    public partial class newvalue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CouponProducts_Products_ProductId",
                table: "CouponProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_CouponUsers_ApplicationUsers_UserId",
                table: "CouponUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_CouponProducts_Products_ProductId",
                table: "CouponProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CouponUsers_ApplicationUsers_UserId",
                table: "CouponUsers",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CouponProducts_Products_ProductId",
                table: "CouponProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_CouponUsers_ApplicationUsers_UserId",
                table: "CouponUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_CouponProducts_Products_ProductId",
                table: "CouponProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CouponUsers_ApplicationUsers_UserId",
                table: "CouponUsers",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
