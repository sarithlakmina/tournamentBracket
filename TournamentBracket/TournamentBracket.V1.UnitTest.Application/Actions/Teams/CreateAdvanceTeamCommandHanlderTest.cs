using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using TournamentBracket.BackEnd.V1.Business.Actions.Teams;
using TournamentBracket.BackEnd.V1.Common.Constants;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Entity;
using TournamentBracket.V1.UnitTest.Application.Common;
using TournamentBracket.V1.UnitTest.Application.Common.Fixtures;

namespace TournamentBracket.V1.UnitTest.Application.Actions.Teams;

public class CreateAdvanceTeamCommandHanlderTest : IClassFixture<TestFixture>
{
    private readonly Mock<ITournamentBracketDbContext> mockDbContext;
    private readonly Mock<IMediator> mediatorMock;
    private readonly IMediator mediator;
    private readonly Guid MockTournamentID;
    public CreateAdvanceTeamCommandHanlderTest(TestFixture testFixture)
    {
        mockDbContext = new Mock<ITournamentBracketDbContext>();
        mediatorMock = new Mock<IMediator>();
        mediator = mediatorMock.Object;
        MockTournamentID = testFixture.TournamentTestID;
    }

    [Fact]
    public async Task AdvanceTeam_R16_MatchFixtures_NoTeams_Error_Test()
    {
        //Arrange
        var requestData = JsonDataReader.ReadAdvanceEventData();
        var teamData = JsonDataReader.ReadSeedFileJsonData();

        mockDbContext.Setup(x => x.Teams).Returns(new Mock<DbSet<Team>>().Object);

        var command = new AdvanceTeamCommand
        {
            AdvanceTeamRequest = requestData
        };

        var handler = new CreateAdvanceTeamCommandHandler(mockDbContext.Object, mediator);


        //Act

        var exception = await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));

        // Assert
        Assert.Contains(ExceptionMessages.TeamNotFoundException, exception.Message);
    }

    [Fact]
    public async Task AdvanceTeam_R16_MatchFixtures_InvalidTeams_Error_Test()
    {
        //Arrange
        var requestData = JsonDataReader.ReadAdvanceEventData();
        var teamData = JsonDataReader.ReadSeedFileJsonData();

        mockDbContext.Setup(x => x.Teams).Returns(new Mock<DbSet<Team>>().Object);

        var command = new AdvanceTeamCommand
        {
            AdvanceTeamRequest = requestData
        };


        var team1 = new Team
        {
            Name = "Test1",
            Seed = "A1",
            TeamID = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
        };

        var team2 = new Team
        {
            Name = "Test2",
            Seed = "A2",
            TeamID = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
        };

        var teamsList = new List<Team>
        {
            team1,team2
        };


        mockDbContext.Setup(x => x.GetAllTeams()).ReturnsAsync(teamsList);

        var handler = new CreateAdvanceTeamCommandHandler(mockDbContext.Object, mediator);


        //Act

        var exception = await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));


        // Assert
        Assert.Contains(ExceptionMessages.ExpectedTeamsAreNoPresentException, exception.Message);
    }

    [Fact]
    public async Task AdvanceTeam_R16_MatchFixtures_MatchesNotFOund_Error_Test()
    {
        //Arrange
        var eventData = JsonDataReader.ReadAdvanceEventData();
        var seedData = JsonDataReader.ReadSeedFileJsonData();

        var command = new AdvanceTeamCommand
        {
            AdvanceTeamRequest = eventData
        };

        // Arrange
        var teamsList = new List<Team>();
        for (int i = 0; i < eventData.Events.Count; i++)
        {
            string eventName = eventData.Events[i];
            string seed = string.Empty;

            if (seedData.ContainsKey(eventName))
            {
                var seedDetails = seedData[eventName];
            }

            var team = new Team
            {
                Name = eventName,
                Seed = seed,
                TeamID = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
            };

            teamsList.Add(team);
        }

        mockDbContext.Setup(x => x.GetAllTeams()).ReturnsAsync(teamsList);

        var handler = new CreateAdvanceTeamCommandHandler(mockDbContext.Object, mediator);

        // Mocking the LINQ query
        var roundOf16Winners = new List<string> { "Test1", "Test2" };


        var roundOf16WinningTeamsTeamIDs = teamsList
            .Where(team => roundOf16Winners.Contains(team.Name))
            .Select(team => team.TeamID)
            .ToList();

        mockDbContext.Setup(x => x.GetAllMatches(roundOf16WinningTeamsTeamIDs))
    .ReturnsAsync(GenerateMockMatches(16, MockTournamentID));

        //Act

        var exception = await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));


        // Assert
        Assert.Contains(ExceptionMessages.MatchNotFoundException, exception.Message);
    }

    [Fact]
    public async Task AdvanceTeam_R16_MatchFixtures_Success_test()
    {

    }

    private List<BackEnd.V1.Common.Entity.Match> GenerateMockMatches(int numberOfMatches, Guid tournamentId)
    {
        var matches = new List<BackEnd.V1.Common.Entity.Match>();
        for (int i = 0; i < numberOfMatches; i++)
        {
            matches.Add(new BackEnd.V1.Common.Entity.Match
            {
                TournamentID = MockTournamentID,
                MatchID = Guid.NewGuid(),
                HomeTeamID = Guid.NewGuid(),
                AwayTeamID = Guid.NewGuid(),
                IsMatchCompleted = false,
            });
        }
        return matches;
    }
}
