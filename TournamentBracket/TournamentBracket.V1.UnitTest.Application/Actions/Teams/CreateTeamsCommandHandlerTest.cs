using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using TournamentBracket.BackEnd.V1.Business.Actions.Matches;
using TournamentBracket.BackEnd.V1.Business.Actions.Tournaments;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.DTO;
using TournamentBracket.BackEnd.V1.Common.Entity;
using TournamentBracket.V1.UnitTest.Application.Common;
using TournamentBracket.V1.UnitTest.Application.Common.Fixtures;

namespace TournamentBracket.V1.UnitTest.Application.Actions.Teams;

public class CreateTeamsCommandHandlerTest : IClassFixture<TestFixture>
{
    private readonly Mock<ITournamentBracketDbContext> mockDbContext;
    private readonly Mock<IMediator> mediatorMock;
    private readonly IMediator mediator;
    private readonly Guid MockTournamentID;

    public CreateTeamsCommandHandlerTest(TestFixture testFixture)
    {
        mockDbContext = new Mock<ITournamentBracketDbContext>();
        mediatorMock = new Mock<IMediator>();
        mediator = mediatorMock.Object;
        MockTournamentID = testFixture.TournamentTestID;
    }

    [Fact]
    public async Task CreateTeams_SuccessTest()
    {
        //Arrange

        mediatorMock.Setup(m => m.Send(It.IsAny<CreateTournamentsCommand>(), default))
            .ReturnsAsync(new CreateTournamentsCommandResult
            {
                TournamentID = MockTournamentID
            });

        mediatorMock.Setup(m => m.Send(It.IsAny<CreateMatchFixturesCommand>(), default))
            .ReturnsAsync(new CreateMatchFixturesCommandResult
            {
                IsSuccess = true
            });

        mockDbContext.Setup(x => x.Teams).Returns(new Mock<DbSet<Team>>().Object);
        mockDbContext.Setup(x => x.TournamentTeamMaps).Returns(new Mock<DbSet<TournamentTeamMap>>().Object);

        var seedDetailsList = JsonDataReader.ReadSeedFileJsonData()["R16"];

        var command = new CreateTeamsCommand
        {
            SeedDetails = JsonDataReader.ReadSeedFileJsonData()
        };

        var teams = new List<Team>();
        var teamResponseList = new List<TeamResponseDto>();
        var seedTeamIDMaps = new Dictionary<Guid, string>();
        var tournamentTeamMaps = new List<TournamentTeamMap>();
        var createdTournamentID = MockTournamentID;

        foreach (var item in seedDetailsList)
        {
            var teamID = Guid.NewGuid();
            var seededTeam = new Team
            {
                TeamID = teamID,
                Name = item.Team,
                Seed = item.Seed,
                CreatedAt = DateTimeOffset.UtcNow
            };
            teams.Add(seededTeam);

            var teamResponse = new TeamResponseDto
            {
                Name = item.Team,
                Seed = item.Seed,
            };
            teamResponseList.Add(teamResponse);

            seedTeamIDMaps.Add(teamID, item.Seed);

            var tournamentTeamMapsToBeAdded = new TournamentTeamMap
            {
                TournamentTeamMapID = Guid.NewGuid(),
                TournamentID = createdTournamentID,
                TeamID = seededTeam.TeamID,
                CreatedAt = DateTimeOffset.UtcNow
            };
            tournamentTeamMaps.Add(tournamentTeamMapsToBeAdded);
        }

        var handler = new CreateTeamsCommandHandler(mediator, mockDbContext.Object);

        //Act 

        var result = await handler.Handle(command, CancellationToken.None);

        var expectedSet = new HashSet<object>(teamResponseList);
        var actualSet = new HashSet<object>(result.Teams);

        // Assert
        Assert.Equal(expectedSet.Count, expectedSet.Count);
    }

}

