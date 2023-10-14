using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Common.Common;

public class Matchup
{
    public Team HomeTeam { get; set; }
    public int HomeTeamGoals { get; set; }
    public Team AwayTeam { get; set; }
    public int AwayTeamGoals { get; set; }
    public Team WinningTeam { get; set; }
}
