using MediatR;
using TournamentBracket.BackEnd.V1.Business.Actions.Definitions;
using TournamentBracket.BackEnd.V1.Business.Actions.Matches;
using TournamentBracket.BackEnd.V1.Common.Common;
using TournamentBracket.BackEnd.V1.Common.Constants;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.DTO;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Business.Actions.Tournaments;

public class CreateTeamsCommand : IRequest<CreateTeamsCommandResult>
{
    public Dictionary<string, List<SeedDetails>> SeedDetails { get; set; }
}

public class CreateTeamsCommandResult
{
    public List<TeamResponseDto> Teams { get; set; }
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
        var teams = new List<Team>();
        var teamResponseDtoList = new List<TeamResponseDto>();
        var tournamentTeamMaps = new List<TournamentTeamMap>();

        var isSuccess = false;

        var seedTeamIDMaps = new Dictionary<Guid, string>();

        var typeOfMatch = request.SeedDetails.Keys.FirstOrDefault()
                 ?? throw new Exception(ExceptionMessages.MatchCategoryNotFound);

        var seedDetailsList = request.SeedDetails[typeOfMatch];

        #region Create Tournament
        var createdTournamentID = await mediator.Send(new CreateTournamentsCommand
        {
            Name = TournamentNames.FIFA2022 // hard coded for now
        });

        #endregion

        foreach (var item in seedDetailsList)
        {
            #region Create Teams

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

            teamResponseDtoList.Add(teamResponse);

            seedTeamIDMaps.Add(teamID, item.Seed);

            #endregion

            #region Create Tournament Team Map

            var tournamentTeamMapsToBeAdded = new TournamentTeamMap
            {
                TournamentTeamMapID = Guid.NewGuid(),
                TournamentID = createdTournamentID.TournamentID,
                TeamID = seededTeam.TeamID,
                CreatedAt = DateTimeOffset.UtcNow
            };
            tournamentTeamMaps.Add(tournamentTeamMapsToBeAdded);

            #endregion
        }

        teamRepository.Teams.AddRange(teams);

        tournamentTeamMapRepository.TournamentTeamMaps.AddRange(tournamentTeamMaps);

        #region Create Match Fixtures

        var createMatchFixtures = await mediator.Send(new CreateMatchFixturesCommand
        {
            MatchType = typeOfMatch,
            TeamsSeedDetails = seedTeamIDMaps,
            TournamentID = createdTournamentID.TournamentID
        }, cancellationToken);

        isSuccess = createMatchFixtures.IsSuccess;
        #endregion        

        if (isSuccess)
            await unitOfWork.SaveChangesAsync();

        return new CreateTeamsCommandResult
        {
            Teams = teamResponseDtoList
        };
    }
}
