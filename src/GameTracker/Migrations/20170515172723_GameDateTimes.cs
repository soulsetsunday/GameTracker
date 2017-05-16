using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameTracker.Migrations
{
    public partial class GameDateTimes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Platforms_PlatformID",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "DaysPlayed",
                table: "Games",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstAdded",
                table: "Games",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "MostRecentlyAdded",
                table: "Games",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "PlatformID",
                table: "Games",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Platforms_PlatformID",
                table: "Games",
                column: "PlatformID",
                principalTable: "Platforms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Platforms_PlatformID",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "DaysPlayed",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "FirstAdded",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "MostRecentlyAdded",
                table: "Games");

            migrationBuilder.AlterColumn<int>(
                name: "PlatformID",
                table: "Games",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Platforms_PlatformID",
                table: "Games",
                column: "PlatformID",
                principalTable: "Platforms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
