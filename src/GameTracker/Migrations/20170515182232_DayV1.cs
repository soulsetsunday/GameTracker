using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GameTracker.Migrations
{
    public partial class DayV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Days",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CalendarDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Days", x => x.ID);
                });

            migrationBuilder.AddColumn<int>(
                name: "DayID",
                table: "Games",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_DayID",
                table: "Games",
                column: "DayID");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Days_DayID",
                table: "Games",
                column: "DayID",
                principalTable: "Days",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Days_DayID",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_DayID",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "DayID",
                table: "Games");

            migrationBuilder.DropTable(
                name: "Days");
        }
    }
}
