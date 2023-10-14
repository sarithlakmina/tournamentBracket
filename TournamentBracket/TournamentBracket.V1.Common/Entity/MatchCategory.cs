namespace TournamentBracket.BackEnd.V1.Common.Entity;

public class MatchCategory
{
    public Guid MatchCategoryID { get; set; }
    public string MatchTypeName { get; set; }

    public virtual ICollection<MatchMatchCategoryMap> MatchMatchCategoryMaps { get; protected set; }
}
