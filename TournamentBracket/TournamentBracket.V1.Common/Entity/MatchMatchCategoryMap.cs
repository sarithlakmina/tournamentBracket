namespace TournamentBracket.BackEnd.V1.Common.Entity;

public class MatchMatchCategoryMap
{
    public Guid MatchMatchCategoryMapID { get; set; }
    public Guid TournamentID { get; set; }
    public Guid MatchID { get; set; }
    public Guid MatchCategoryID { get; set; }

    public virtual MatchCategory MatchCategory { get; set; }
    public virtual Match Match { get; set; }
    public virtual Tournament Tournament { get; set; }
}
