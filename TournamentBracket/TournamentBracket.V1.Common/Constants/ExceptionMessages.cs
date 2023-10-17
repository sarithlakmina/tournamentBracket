namespace TournamentBracket.BackEnd.V1.Common.Constants;

public static class ExceptionMessages
{
    public const string SeedDataNotFoundException = "Seed / Team data cannot be null";

    public const string TeamNotFoundException = "Team not found";

    public const string TournamentNotFoundException = "Tournament not found";

    public const string MatchNotFoundException = "No active matches found for this team";

    public const string NumberOfTeamsIncorrectException = "The number of teams is wrong.";

    public const string TournamentHasNoWinnerFoundException = "Tournament has no winner yet";

    public const string TournamentNameCannotBeEmpty = "Tournament Name cannot be empty";

    public const string TournamentNameShouldBeUniqueException = "Tournament Name should be unique";

    public const string ExpectedTeamsAreNoPresentException = "Expected teams are not present";

    public const string MatchCategoryNotFound = "Match category not found";
}
