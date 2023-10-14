namespace TournamentBracket.BackEnd.V1.Common.Entity;

public class TournamentMatchMap
{
    public Guid TournamentMatchMapID { get; set; }
    public Guid TournamentID { get; set; }
    public Guid MatchID { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public bool IsMatchCompleted { get; set; }

    public Guid TeamID { get; set; }

    public virtual Tournament Tournament { get; set; }
    public virtual Match Match { get; set; }
}
