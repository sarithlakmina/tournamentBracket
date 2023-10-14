namespace TournamentBracket.BackEnd.V1.Common.Entity;

public class Match
{
    public Guid TournamentID { get; set; }
    public Guid MatchID { get; set; }
    public Guid HomeTeamID { get; set; }
    public Guid AwayTeamID { get; set; }
    public Guid? WinningTeamID { get; set; }
    public bool IsMatchCompleted { get; set; } = false;

    public virtual ICollection<TournamentMatchMap> TournamentMatchMaps { get; protected set; }
    public virtual ICollection<MatchMatchCategoryMap> MatchMatchCategoryMaps { get; protected set; }
    public virtual Tournament Tournament { get; set; }
}
