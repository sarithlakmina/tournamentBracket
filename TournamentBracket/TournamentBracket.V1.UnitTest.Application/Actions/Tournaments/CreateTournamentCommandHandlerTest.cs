using Microsoft.EntityFrameworkCore;
using Moq;
using TournamentBracket.BackEnd.V1.Business.Actions.Tournaments;
using TournamentBracket.BackEnd.V1.Common.Constants;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Entity;
using TournamentBracket.V1.UnitTest.Application.Common;

namespace TournamentBracket.V1.UnitTest.Application.Actions.Tournaments;

public class CreateTournamentCommandHandlerTest
{

    private readonly Mock<ITournamentBracketDbContext> mockDbContext;

    public CreateTournamentCommandHandlerTest()
    {
        mockDbContext = new Mock<ITournamentBracketDbContext>();
    }

    [Fact]
    public async Task CreateTournament_UniqueNameTest()
    {
        //Arrange       

        var command = new CreateTournamentsCommand
        {
            Name = "FIFA 2022"
        };

        mockDbContext.Setup(
            x => x.IsTournamentNameUniqueToCreate(
                It.IsAny<string>())).ReturnsAsync(false);

        var handler = new CreateTournamentsCommandHandler(mockDbContext.Object);

        //Act

        var exception = await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));

        //Assert      

        Assert.Contains(ExceptionMessages.TournamentNameCannotBeEmpty, exception.Message);

    }

    [Fact]
    public async Task CreateTournament_EmptyTest()
    {
        //Arrange       

        var command = new CreateTournamentsCommand
        {
            Name = ""
        };

        mockDbContext.Setup(
            x => x.IsTournamentNameUniqueToCreate(
                It.IsAny<string>())).ReturnsAsync(false);

        var handler = new CreateTournamentsCommandHandler(mockDbContext.Object);

        //Act

        var exception = await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));

        //Assert      

        Assert.Contains(ExceptionMessages.TournamentNameCannotBeEmpty, exception.Message);

    }

    [Fact]
    public async Task CreateTournament_ShouldPassTest()
    {
        //Arrange        

        var command = new CreateTournamentsCommand
        {
            Name = "Cricket 2023"
        };
        mockDbContext.Setup(x => x.Tournaments).Returns(new Mock<DbSet<Tournament>>().Object);

        mockDbContext.Setup(
            x => x.IsTournamentNameUniqueToCreate(
                It.IsAny<string>())).ReturnsAsync(true);

        var handler = new CreateTournamentsCommandHandler(mockDbContext.Object);

        //Act

        var result = await handler.Handle(command, CancellationToken.None);

        //Assert

        Assert.NotNull(result);
        Assert.True(Guid.TryParse(result.TournamentID.ToString(), out _));
        TestHelperData.TournamentTestID = result.TournamentID;

    }

    [Fact]
    public async Task CreateTournament_ShouldCallAddMethodTest()
    {
        //Arrange      

        var command = new CreateTournamentsCommand
        {
            Name = "Cricket 2023"
        };
        mockDbContext.Setup(x => x.Tournaments).Returns(new Mock<DbSet<Tournament>>().Object);

        mockDbContext.Setup(
            x => x.IsTournamentNameUniqueToCreate(
                It.IsAny<string>())).ReturnsAsync(true);

        var handler = new CreateTournamentsCommandHandler(mockDbContext.Object);

        //Act

        var result = await handler.Handle(command, CancellationToken.None);

        //Assert

        mockDbContext.Verify(x => x.Tournaments.Add(It.Is<Tournament>(t => t.TournamentName == command.Name)), Times.Once);

    }
}
