using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentBracket.BackEnd.V1.Persistence.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Seed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamID);
                });

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    TournamentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TournamentName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Winner = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SecondPlace = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ThirdPlace = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.TournamentID);
                });

            migrationBuilder.CreateTable(
                name: "MatchCategories",
                columns: table => new
                {
                    MatchCategoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchTypeName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TournamentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchCategories", x => x.MatchCategoryID);
                    table.ForeignKey(
                        name: "FK_MatchCategories_Tournaments_TournamentID",
                        column: x => x.TournamentID,
                        principalTable: "Tournaments",
                        principalColumn: "TournamentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    MatchID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TournamentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HomeTeamID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AwayTeamID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WinningTeamID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsMatchCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.MatchID);
                    table.ForeignKey(
                        name: "FK_Matches_Tournaments_TournamentID",
                        column: x => x.TournamentID,
                        principalTable: "Tournaments",
                        principalColumn: "TournamentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TournamentTeamMaps",
                columns: table => new
                {
                    TournamentTeamMapID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TournamentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsElemenated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentTeamMaps", x => x.TournamentTeamMapID);
                    table.ForeignKey(
                        name: "FK_TournamentTeamMaps_Teams_TeamID",
                        column: x => x.TeamID,
                        principalTable: "Teams",
                        principalColumn: "TeamID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentTeamMaps_Tournaments_TournamentID",
                        column: x => x.TournamentID,
                        principalTable: "Tournaments",
                        principalColumn: "TournamentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MatchMatchCategoryMaps",
                columns: table => new
                {
                    MatchMatchCategoryMapID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TournamentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchCategoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsMatchCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchMatchCategoryMaps", x => x.MatchMatchCategoryMapID);
                    table.ForeignKey(
                        name: "FK_MatchMatchCategoryMaps_MatchCategories_MatchCategoryID",
                        column: x => x.MatchCategoryID,
                        principalTable: "MatchCategories",
                        principalColumn: "MatchCategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchMatchCategoryMaps_Matches_MatchID",
                        column: x => x.MatchID,
                        principalTable: "Matches",
                        principalColumn: "MatchID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchMatchCategoryMaps_Tournaments_TournamentID",
                        column: x => x.TournamentID,
                        principalTable: "Tournaments",
                        principalColumn: "TournamentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentMatchMaps",
                columns: table => new
                {
                    TournamentMatchMapID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TournamentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsMatchCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentMatchMaps", x => x.TournamentMatchMapID);
                    table.ForeignKey(
                        name: "FK_TournamentMatchMaps_Matches_MatchID",
                        column: x => x.MatchID,
                        principalTable: "Matches",
                        principalColumn: "MatchID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentMatchMaps_Tournaments_TournamentID",
                        column: x => x.TournamentID,
                        principalTable: "Tournaments",
                        principalColumn: "TournamentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchCategories_TournamentID",
                table: "MatchCategories",
                column: "TournamentID");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TournamentID",
                table: "Matches",
                column: "TournamentID");

            migrationBuilder.CreateIndex(
                name: "IX_MatchMatchCategoryMaps_MatchCategoryID",
                table: "MatchMatchCategoryMaps",
                column: "MatchCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_MatchMatchCategoryMaps_MatchID",
                table: "MatchMatchCategoryMaps",
                column: "MatchID");

            migrationBuilder.CreateIndex(
                name: "IX_MatchMatchCategoryMaps_MatchMatchCategoryMapID",
                table: "MatchMatchCategoryMaps",
                column: "MatchMatchCategoryMapID");

            migrationBuilder.CreateIndex(
                name: "IX_MatchMatchCategoryMaps_TournamentID",
                table: "MatchMatchCategoryMaps",
                column: "TournamentID");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentMatchMaps_MatchID",
                table: "TournamentMatchMaps",
                column: "MatchID");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentMatchMaps_TournamentID",
                table: "TournamentMatchMaps",
                column: "TournamentID");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentMatchMaps_TournamentMatchMapID",
                table: "TournamentMatchMaps",
                column: "TournamentMatchMapID");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTeamMaps_TeamID",
                table: "TournamentTeamMaps",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTeamMaps_TournamentID",
                table: "TournamentTeamMaps",
                column: "TournamentID");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTeamMaps_TournamentTeamMapID",
                table: "TournamentTeamMaps",
                column: "TournamentTeamMapID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchMatchCategoryMaps");

            migrationBuilder.DropTable(
                name: "TournamentMatchMaps");

            migrationBuilder.DropTable(
                name: "TournamentTeamMaps");

            migrationBuilder.DropTable(
                name: "MatchCategories");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Tournaments");
        }
    }
}
