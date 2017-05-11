using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameTracker.Migrations
{
    public partial class GameImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameImage_GameImagesID",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameImage",
                table: "GameImage");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameImages",
                table: "GameImage",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameImages_GameImagesID",
                table: "Games",
                column: "GameImagesID",
                principalTable: "GameImage",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameTable(
                name: "GameImage",
                newName: "GameImages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameImages_GameImagesID",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameImages",
                table: "GameImages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameImage",
                table: "GameImages",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameImage_GameImagesID",
                table: "Games",
                column: "GameImagesID",
                principalTable: "GameImages",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameTable(
                name: "GameImages",
                newName: "GameImage");
        }
    }
}
