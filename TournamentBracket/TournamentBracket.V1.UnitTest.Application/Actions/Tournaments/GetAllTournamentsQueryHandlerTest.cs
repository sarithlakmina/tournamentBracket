using Moq;
using TournamentBracket.BackEnd.V1.Business.Actions.Tournaments;
using TournamentBracket.BackEnd.V1.Common.Constants;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Entity;
using TournamentBracket.V1.UnitTest.Application.Common;

namespace TournamentBracket.V1.UnitTest.Application.Actions.Tournaments;

public class GetAllTournamentsQueryHandlerTest
{
    private readonly Mock<ITournamentBracketDbContext> mockDbContext;

    public GetAllTournamentsQueryHandlerTest()
    {
        mockDbContext = new Mock<ITournamentBracketDbContext>();
    }

    [Fact]
    public async Task GetTournamentWinner_EmptyRequestTest()
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
    public async Task GetTournamentWinner_WithValidIDTest()
    {
        //Arrange       

        var query = new GetTournamentWinnerQuery
        {
            TournamentID = TestHelperData.TournamentTestID
        };


        var testString = new Tournament
        {
            TournamentID = TestHelperData.TournamentTestID,
            TournamentName = "Cricket 2023"
        };

        mockDbContext.Setup(x => x.GetTournament(TestHelperData.TournamentTestID)).ReturnsAsync(testString);

        var handler = new GetTournamentWinnerQueryHandler(mockDbContext.Object);

        //Act

        var result = await handler.Handle(query, CancellationToken.None);

        //Assert      

        Assert.NotNull(result.WinnerName);

    }
}
