namespace TournamentBracket.V1.UnitTest.Application.Common;

public static class TestHelperData
{
    public static Guid TournamentTestID { get; set; }

    public static string Winner = JsonDataReader.ReadAdvanceEventData().Winner;
}
