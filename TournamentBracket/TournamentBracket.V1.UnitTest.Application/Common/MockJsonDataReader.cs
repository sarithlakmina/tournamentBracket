using System.Text.Json;
using TournamentBracket.BackEnd.V1.Common.Common;
using TournamentBracket.BackEnd.V1.Common.Model;

namespace TournamentBracket.V1.UnitTest.Application.Common;

public static class MockJsonDataReader
{
    public static Dictionary<string, List<SeedDetails>> ReadSeedFileData()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var jsonFilePath = Path.Combine(currentDirectory, "Data", "SeedFile.json");

        var jsonData = File.ReadAllText(jsonFilePath);

        // Deserializing JSON to Dictionary<string, List<SeedDetails>>
        var deserializedData = JsonSerializer.Deserialize<Dictionary<string, List<SeedDetails>>>(jsonData, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return deserializedData;
    }

    public static AdvanceTeamRequestModel ReadAdvanceEventData()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var jsonFilePath = Path.Combine(currentDirectory, "Data", "AdvanceEvents.json");

        var jsonData = File.ReadAllText(jsonFilePath);
        var returnData = JsonSerializer.Deserialize<AdvanceTeamRequestModel>(jsonData, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return returnData;
    }
}
