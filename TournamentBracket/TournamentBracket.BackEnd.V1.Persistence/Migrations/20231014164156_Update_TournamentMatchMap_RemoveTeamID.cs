using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentBracket.BackEnd.V1.Persistence.Migrations
{
    public partial class Update_TournamentMatchMap_RemoveTeamID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamID",
                table: "TournamentMatchMaps");

            migrationBuilder.AddColumn<Guid>(
                name: "TournamentID",
                table: "MatchCategories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MatchCategories_TournamentID",
                table: "MatchCategories",
                column: "TournamentID");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchCategories_Tournaments_TournamentID",
                table: "MatchCategories",
                column: "TournamentID",
                principalTable: "Tournaments",
                principalColumn: "TournamentID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchCategories_Tournaments_TournamentID",
                table: "MatchCategories");

            migrationBuilder.DropIndex(
                name: "IX_MatchCategories_TournamentID",
                table: "MatchCategories");

            migrationBuilder.DropColumn(
                name: "TournamentID",
                table: "MatchCategories");

            migrationBuilder.AddColumn<Guid>(
                name: "TeamID",
                table: "TournamentMatchMaps",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
