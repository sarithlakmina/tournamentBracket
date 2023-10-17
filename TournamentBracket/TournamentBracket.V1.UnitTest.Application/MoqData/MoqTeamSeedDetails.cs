using TournamentBracket.V1.UnitTest.Application.Common;

namespace TournamentBracket.V1.UnitTest.Application.MoqData;

public static class MoqTeamSeedDetails
{
    public static Dictionary<Guid, string> TeamSeedDetails()
    {
        var moqDictionary = new Dictionary<Guid, string>();
        var seedList = JsonDataReader.ReadSeedFileJsonData();
        var seeds = seedList.SelectMany(a => a.Value).ToList();

        foreach (var seed in seeds)
        {
            moqDictionary.Add(Guid.NewGuid(), seed.Seed);
        }

        return moqDictionary;
    }
}
