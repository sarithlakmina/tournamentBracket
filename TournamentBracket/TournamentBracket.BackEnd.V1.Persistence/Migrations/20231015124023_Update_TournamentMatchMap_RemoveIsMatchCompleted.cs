using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentBracket.BackEnd.V1.Persistence.Migrations
{
    public partial class Update_TournamentMatchMap_RemoveIsMatchCompleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMatchCompleted",
                table: "TournamentMatchMaps");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMatchCompleted",
                table: "TournamentMatchMaps",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
