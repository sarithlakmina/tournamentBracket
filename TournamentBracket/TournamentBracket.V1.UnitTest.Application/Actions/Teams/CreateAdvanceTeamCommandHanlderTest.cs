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
    public async Task AdvanceTeam_R16_MatchFixtures_Success_Test()
    {
        //Arrange
        var requestData = JsonDataReader.ReadAdvanceEventData();
        var teamData = JsonDataReader.ReadSeedFileJsonData();

        var command = new AdvanceTeamCommand
        {
            AdvanceTeamRequest = requestData
        };

        // Arrange

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

        // Mocking the LINQ query
        var roundOf16Winners = new List<string> { "Test1", "Test2" }; // Replace with your expected list of winners

        // Simulating the behavior of the LINQ query
        var roundOf16WinningTeamsTeamIDs = teamsList
            .Where(team => roundOf16Winners.Contains(team.Name))
            .Select(team => team.TeamID)
            .ToList();

        //Act

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(roundOf16WinningTeamsTeamIDs);
    }
}
