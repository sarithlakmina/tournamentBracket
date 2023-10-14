using MediatR;
using TournamentBracket.BackEnd.V1.Business.Actions.Definitions;
using TournamentBracket.BackEnd.V1.Business.Actions.Matches;
using TournamentBracket.BackEnd.V1.Common.Common;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Business.Actions.Tournaments;

public class CreateTeamsCommand : IRequest<CreateTeamsCommandResult>
{
    public Dictionary<string, List<SeedDetails>> SeedDetails { get; set; }
}

public class CreateTeamsCommandResult
{
    public List<Team> Teams { get; set; }
}

public class CreateTeamsCommandHandler : BackEndGenericHandler, IRequestHandler<CreateTeamsCommand, CreateTeamsCommandResult>
{
    private readonly IMediator mediator;

    public CreateTeamsCommandHandler(IMediator mediator, ITournamentBracketDbContext dbContext) : base(dbContext)
    {
        this.mediator = mediator;
    }

    public async Task<CreateTeamsCommandResult> Handle(CreateTeamsCommand request, CancellationToken cancellationToken)
    {
        var result = new CreateTeamsCommandResult();
        var teams = new List<Team>();
        var tournamentTeamMaps = new List<TournamentTeamMap>();
        var tournamentID = Guid.NewGuid();

        var isSuccess = false;

        var seedTeamIDMaps = new Dictionary<Guid, string>();

        var typeOfMatch = request.SeedDetails.Keys.FirstOrDefault();
        var seedDetailsList = request.SeedDetails[typeOfMatch];

        #region Create Tournament
        var tournament = new Tournament
        {
            TournamentID = tournamentID,
            TournamentName = "FIFA 2022",  // hardcoded for now.
        };

        tournamentRepository.Tournaments.Add(tournament);

        #endregion


        foreach (var item in seedDetailsList)
        {
            #region Seed Teams

            var teamID = Guid.NewGuid();
            var seededTeam = new Team
            {
                TeamID = teamID,
                Name = item.Team,
                Seed = item.Seed,
                CreatedAt = DateTimeOffset.UtcNow
            };
            teams.Add(seededTeam);

            seedTeamIDMaps.Add(teamID, item.Seed);
            #endregion

            #region Create Tournament Team Map

            var tournamentTeamMapsToBeAdded = new TournamentTeamMap
            {
                TournamentTeamMapID = Guid.NewGuid(),
                TournamentID = tournamentID,
                TeamID = seededTeam.TeamID,
                CreatedAt = DateTimeOffset.UtcNow
            };
            tournamentTeamMaps.Add(tournamentTeamMapsToBeAdded);

            #endregion
        }
        result.Teams = teams;

        teamRepository.Teams.AddRange(teams);

        tournamentTeamMapRepository.TournamentTeamMaps.AddRange(tournamentTeamMaps);

        #region Create Match Fixtures

        var createMatchFixtures = await mediator.Send(new CreateMatchFixturesCommand
        {
            MatchType = typeOfMatch,
            TeamsSeedDetails = seedTeamIDMaps,
            TournamentID = tournamentID
        }, cancellationToken);

        isSuccess = createMatchFixtures.IsSuccess;
        #endregion        

        if (isSuccess)
            await unitOfWork.SaveChangesAsync();

        return result;
    }
}
