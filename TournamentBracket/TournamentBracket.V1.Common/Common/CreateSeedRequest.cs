namespace TournamentBracket.BackEnd.V1.Common.Common;

public class CreateSeedRequest
{
    public List<SeedDetails> SeedDetails { get; set; }
}

public class Temp
{
    public Dictionary<string, List<SeedDetails>> Data { get; set; }
}