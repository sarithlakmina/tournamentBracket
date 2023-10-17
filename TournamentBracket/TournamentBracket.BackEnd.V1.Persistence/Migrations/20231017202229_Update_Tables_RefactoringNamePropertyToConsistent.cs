using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentBracket.BackEnd.V1.Persistence.Migrations
{
    public partial class Update_Tables_RefactoringNamePropertyToConsistent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsElemenated",
                table: "TournamentTeamMaps");

            migrationBuilder.DropColumn(
                name: "IsMatchCompleted",
                table: "MatchMatchCategoryMaps");

            migrationBuilder.RenameColumn(
                name: "TournamentName",
                table: "Tournaments",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Tournaments",
                newName: "TournamentName");

            migrationBuilder.AddColumn<bool>(
                name: "IsElemenated",
                table: "TournamentTeamMaps",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMatchCompleted",
                table: "MatchMatchCategoryMaps",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
