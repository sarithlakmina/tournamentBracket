namespace TournamentBracket.BackEnd.V1.Common.Entity;

public class Tournament
{
    public Guid TournamentID { get; set; }
    public string TournamentName { get; set; }
    public Guid? Winner { get; set; }
    public Guid? SecondPlace { get; set; }
    public Guid? ThirdPlace { get; set; }

    public virtual ICollection<TournamentTeamMap> TournamentTeamMaps { get; protected set; }
    public virtual ICollection<TournamentMatchMap> TournamentMatchMaps { get; protected set; }
    public virtual ICollection<MatchMatchCategoryMap> MatchMatchCategoryMaps { get; protected set; }

}
