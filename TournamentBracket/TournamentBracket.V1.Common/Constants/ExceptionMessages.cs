namespace TournamentBracket.BackEnd.V1.Common.Constants;

public static class ExceptionMessages
{
    public const string SeedDataNotFoundException = "Seed / Team data cannot be null";
    public const string TeamNotFoundException = "Team not found";
    public const string MatchNotFoundException = "No active matches found for this team";
    public const string NumberOfTeamsIncorrectException = "The number of teams is wrong.";
}
