using Moq;
using TournamentBracket.BackEnd.V1.Business.Actions.Tournaments;
using TournamentBracket.BackEnd.V1.Common.Constants;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Entity;
using TournamentBracket.V1.UnitTest.Application.Common.Fixtures;

namespace TournamentBracket.V1.UnitTest.Application.Actions.Tournaments;

public class GetAllTournamentsQueryHandlerTest : IClassFixture<TestFixture>
{
    private readonly Mock<ITournamentBracketDbContext> mockDbContext;

    private readonly Guid MockTournamentID;

    public GetAllTournamentsQueryHandlerTest(TestFixture testFixture)
    {
        mockDbContext = new Mock<ITournamentBracketDbContext>();
        MockTournamentID = testFixture.TournamentTestID;

    }

    [Fact]
    public async Task GetTournamentWinner_EmptyTournamentIDRequestTest()
    {
        //Arrange       

        var command = new GetTournamentWinnerQuery
        {
            TournamentID = Guid.NewGuid(),
        };

        var handler = new GetTournamentWinnerQueryHandler(mockDbContext.Object);

        //Act

        var exception = await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));

        //Assert      

        Assert.Contains(ExceptionMessages.TournamentNotFoundException, exception.Message);

    }

    [Fact]
    public async Task GetTournamentWinner_WithValidID_NoWinnersTest()
    {
        //Arrange       

        var query = new GetTournamentWinnerQuery
        {
            TournamentID = MockTournamentID
        };

        // mockDbContext.Setup(x => x.Tournaments).Returns(new Mock<DbSet<Tournament>>().Object);

        var testTournament = new Tournament
        {
            TournamentID = MockTournamentID,
            TournamentName = "Cricket 2023",
            Winner = Guid.NewGuid()
        };

        mockDbContext.Setup(x => x.GetTournament(MockTournamentID)).ReturnsAsync(testTournament);

        var handler = new GetTournamentWinnerQueryHandler(mockDbContext.Object);

        //Act

        var exception = await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(query, CancellationToken.None));

        //Assert      

        Assert.Contains(ExceptionMessages.TournamentHasNoWinnerFoundException, exception.Message);

    }
}
