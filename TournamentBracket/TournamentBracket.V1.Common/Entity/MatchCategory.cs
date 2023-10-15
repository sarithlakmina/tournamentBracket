namespace TournamentBracket.BackEnd.V1.Common.Entity;

public class MatchCategory
{
    public Guid MatchCategoryID { get; set; }
    public string Name { get; set; }
    public Guid TournamentID { get; set; }

    public virtual ICollection<MatchMatchCategoryMap> MatchMatchCategoryMaps { get; protected set; }
    public virtual Tournament Tournament { get; set; }
}
