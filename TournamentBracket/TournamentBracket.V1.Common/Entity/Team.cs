namespace TournamentBracket.BackEnd.V1.Common.Entity;

public class Team
{
    public Guid TeamID { get; set; }
    public string Name { get; set; }
    public string Seed { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public virtual ICollection<TournamentTeamMap> TournamentTeamMaps { get; protected set; }

}
