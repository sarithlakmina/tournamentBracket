namespace TournamentBracket.BackEnd.V1.Common.DTO;

public class WinnerDto
{
    //t.Seed, t.Name, t.TeamID
    public string Name { get; set; }
    public string Seed { get; set; }
    public Guid TeamID { get; set; }
}
