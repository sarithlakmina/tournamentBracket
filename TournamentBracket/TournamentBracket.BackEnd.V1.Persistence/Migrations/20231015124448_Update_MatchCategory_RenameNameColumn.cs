using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentBracket.BackEnd.V1.Persistence.Migrations
{
    public partial class Update_MatchCategory_RenameNameColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MatchTypeName",
                table: "MatchCategories",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "MatchCategories",
                newName: "MatchTypeName");
        }
    }
}
