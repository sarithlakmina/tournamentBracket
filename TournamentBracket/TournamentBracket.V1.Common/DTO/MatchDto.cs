namespace TournamentBracket.BackEnd.V1.Common.DTO;

public class MatchDto
{
    public Guid MatchID { get; set; }
    public Guid WinnderTeamID { get; set; }
    public string Seed { get; set; }
}
