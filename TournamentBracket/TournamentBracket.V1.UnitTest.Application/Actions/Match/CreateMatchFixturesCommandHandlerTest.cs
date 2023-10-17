using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using TournamentBracket.BackEnd.V1.Business.Actions.Matches;
using TournamentBracket.BackEnd.V1.Common.Constants;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Entity;
using TournamentBracket.V1.UnitTest.Application.Common;
using TournamentBracket.V1.UnitTest.Application.Common.Fixtures;
using TournamentBracket.V1.UnitTest.Application.MoqData;

namespace TournamentBracket.V1.UnitTest.Application.Actions.Match;

public class CreateMatchFixturesCommandHandlerTest : IClassFixture<TestFixture>
{
    private readonly Mock<ITournamentBracketDbContext> mockDbContext;
    private readonly Mock<IMediator> mediatorMock;
    private readonly IMediator mediator;
    private readonly Guid MockTournamentID;
    public CreateMatchFixturesCommandHandlerTest(TestFixture testFixture)
    {
        mockDbContext = new Mock<ITournamentBracketDbContext>();
        mediatorMock = new Mock<IMediator>();
        mediator = mediatorMock.Object;
        MockTournamentID = testFixture.TournamentTestID;
    }

    [Fact]
    public async Task CreateMatchFixtures_MatchCategory_FailureTest()
    {
        //Arrange
        var TeamSeedsDetails = JsonDataReader.ReadSeedFileJsonData().Values;
        var tournamentID = MockTournamentID;
        string matchType = JsonDataReader.ReadSeedFileJsonData().Keys.First();

        var moqTeamSeeds = MoqTeamSeedDetails.TeamSeedDetails();

        moqTeamSeeds.Remove(moqTeamSeeds.Keys.First());

        var query = new CreateMatchFixturesCommand
        {
            TournamentID = tournamentID,
            MatchType = matchType,
            TeamsSeedDetails = moqTeamSeeds
        };

        var handler = new CreateMatchFixturesCommandHandler(mockDbContext.Object);

        //Act

        var exception = await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(query, CancellationToken.None));

        //Assert      

        Assert.Contains(ExceptionMessages.NumberOfTeamsIncorrectException, exception.Message);
    }

    [Fact]
    public async Task CreateMatchFixtures_MatchCategory_SuccessTest()
    {
        //Arrange
        var TeamSeedsDetails = JsonDataReader.ReadSeedFileJsonData().Values;
        string matchType = JsonDataReader.ReadSeedFileJsonData().Keys.First();

        var moqTeamSeeds = MoqTeamSeedDetails.TeamSeedDetails();

        mockDbContext.Setup(x => x.Matches).Returns(new Mock<DbSet<BackEnd.V1.Common.Entity.Match>>().Object);
        mockDbContext.Setup(x => x.MatchCategories).Returns(new Mock<DbSet<MatchCategory>>().Object);
        mockDbContext.Setup(x => x.MatchMatchCategoryMaps).Returns(new Mock<DbSet<MatchMatchCategoryMap>>().Object);
        mockDbContext.Setup(x => x.TournamentMatchMaps).Returns(new Mock<DbSet<TournamentMatchMap>>().Object);

        var query = new CreateMatchFixturesCommand
        {
            TournamentID = MockTournamentID,
            MatchType = matchType,
            TeamsSeedDetails = moqTeamSeeds
        };

        var handler = new CreateMatchFixturesCommandHandler(mockDbContext.Object);

        //Act

        var result = await handler.Handle(query, CancellationToken.None);

        //Assert      

        Assert.True(result.IsSuccess);
    }

}
