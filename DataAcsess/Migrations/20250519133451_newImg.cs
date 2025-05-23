using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcsess.Migrations
{
    public partial class newImg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "ProductImages",
                newName: "ContentType");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "ProductImages",
                type: "longblob",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "ProductImages");

            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "ProductImages",
                newName: "ImageUrl");
        }
    }
}
