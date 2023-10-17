using Moq;
using TournamentBracket.BackEnd.V1.Common.Entity;
using TournamentBracket.BackEnd.V1.Common.Repository;

namespace TournamentBracket.V1.UnitTest.Application.Mocks
{
    public static class MockTeamsRepository
    {
        public static Mock<ITeamRepository> GetMockTeams()
        {

            var teams = new List<Team>
            {
                new Team
                {
                    CreatedAt = DateTime.Now,
                    Name = "Test",
                    Seed = "A1",
                    TeamID = Guid.NewGuid(),
                },
                new Team
                {
                    CreatedAt = DateTime.Now,
                    Name = "Test1",
                    Seed = "A2",
                    TeamID = Guid.NewGuid(),
                }
            };
            var test = new Mock<ITeamRepository>();

            test.Setup(r => r.GetAllTeams()).ReturnsAsync(teams);
            return test;
        }
    }
}
