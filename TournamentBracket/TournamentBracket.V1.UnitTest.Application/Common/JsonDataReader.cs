using System.Text.Json;
using TournamentBracket.BackEnd.V1.Common.Common;
using TournamentBracket.BackEnd.V1.Common.Model;

namespace TournamentBracket.V1.UnitTest.Application.Common;

public static class JsonDataReader
{
    public static Dictionary<string, List<SeedDetails>> ReadSeedFileJsonData()
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
        var jsonFilePath = Path.Combine(currentDirectory, "Data", "SeedFile.json");

        var jsonData = File.ReadAllText(jsonFilePath);
        var data = JsonSerializer.Deserialize<AdvanceTeamRequestModel>(jsonData, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return data;
    }
}
