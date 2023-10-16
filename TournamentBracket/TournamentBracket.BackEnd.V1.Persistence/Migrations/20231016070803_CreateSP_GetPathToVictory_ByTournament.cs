using Microsoft.EntityFrameworkCore.Migrations;
using TournamentBracket.BackEnd.V1.Persistence.Properties;

#nullable disable

namespace TournamentBracket.BackEnd.V1.Persistence.Migrations
{
    public partial class CreateSP_GetPathToVictory_ByTournament : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Resources.SPCreate_GetPathToVictory_Up);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
