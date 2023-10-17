using Moq;
using TournamentBracket.BackEnd.V1.Common.Entity;
using TournamentBracket.BackEnd.V1.Common.Repository;

namespace TournamentBracket.V1.UnitTest.Application.Mocks;

public static class MockTournamentsRepository
{
    public static Mock<ITournamentRepository> GetMockTournaments()
    {
        var winningTeam = Guid.NewGuid();
        var tournamentID = Guid.NewGuid();

        var tournaments = new List<Tournament>
            {
                new Tournament
                {
                    TournamentID = Guid.NewGuid(),
                    TournamentName = "Test1",
                },
                new Tournament
                {
                    TournamentID = Guid.NewGuid(),
                    TournamentName = "Test2",
                }
            };
        var test = new Mock<ITournamentRepository>();

        test.Setup(r => r.GetAllTournaments()).ReturnsAsync(tournaments);
        return test;
    }
}
