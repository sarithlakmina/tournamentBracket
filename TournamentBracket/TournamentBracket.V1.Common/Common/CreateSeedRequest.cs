namespace TournamentBracket.BackEnd.V1.Common.Common;

public class CreateSeedRequest
{
    public Dictionary<string, List<SeedDetails>> SeedCategories { get; set; }
}