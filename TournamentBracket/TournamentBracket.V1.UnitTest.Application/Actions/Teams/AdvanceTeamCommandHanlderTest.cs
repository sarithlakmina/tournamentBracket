using MediatR;
using Moq;
using TournamentBracket.BackEnd.V1.Common.Database;

namespace TournamentBracket.V1.UnitTest.Application.Actions.Teams;

public class AdvanceTeamCommandHanlderTest
{
    private readonly Mock<ITournamentBracketDbContext> mockDbContext;
    private readonly Mock<IMediator> mediatorMock;
    private readonly IMediator mediator;
    public AdvanceTeamCommandHanlderTest()
    {
        mockDbContext = new Mock<ITournamentBracketDbContext>();
        mediatorMock = new Mock<IMediator>();
        mediator = mediatorMock.Object;
    }



    [Fact]
    public async Task AdvanceTeam_R16SuccessTest()
    {
        ////Arrange
        //var requestData = JsonDataReader.ReadAdvanceEventData();

        //var teamRepositoryMock = new Mock<ITeamRepository>();
        //var matchRepositoryMock = new Mock<IMatchRepository>();

        //var r16Teams = new List<String>();

        //var teams = teamRepositoryMock.Object.Teams;



        //// Set up sample data
        //foreach (var item in requestData.Events)
        //{
        //    r16Teams.Add(item);
        //}


        //teamRepositoryMock.Setup(x => x.GetAllTeams()).ReturnsAsync(teams);

        ////var matches = new List<Match>
        ////{
        ////    // Add your sample matches here
        ////};
        ////matchRepositoryMock.Setup(x => x.GetAllMatches(It.IsAny<List<Guid>>())).ReturnsAsync(matches);

        ////// Your other setup here

        ////// Instantiate the class that contains the method to be tested
        ////var yourClass = new YourClass(teamRepositoryMock.Object, matchRepositoryMock.Object);

        ////// Call the method to be tested
        ////// You might need to adjust the arguments based on your method signature
        ////await yourClass.YourMethod(yourRequestObject);

        //// Add your assertions here based on the expected behavior of your method
        //// Assert...
    }
}
