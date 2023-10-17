namespace TournamentBracket.BackEnd.V1.Common.Entity;
public class TournamentTeamMap
{
    public Guid TournamentTeamMapID { get; set; }
    public Guid TournamentID { get; set; }
    public Guid TeamID { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public virtual Tournament Tournament { get; set; }
    public virtual Team Team { get; set; }
}
