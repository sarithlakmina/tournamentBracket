using MediatR;
using TournamentBracket.BackEnd.V1.Business.Actions.Definitions;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Business.Actions.Matches
{

    public class CreateMatchFixturesCommand : IRequest<CreateMatchFixturesCommandResult>
    {
        public Guid TournamentID { get; set; }
        public string MatchType { get; set; }
        public Dictionary<Guid, string> TeamsSeedDetails { get; set; }
    }

    public class CreateMatchFixturesCommandResult
    {
        public bool IsSuccess { get; set; }
    }

    public class CreateMatchFixturesCommandHandler : BackEndGenericHandler, IRequestHandler<CreateMatchFixturesCommand, CreateMatchFixturesCommandResult>
    {
        public CreateMatchFixturesCommandHandler(ITournamentBracketDbContext dbContext) : base(dbContext) { }

        public async Task<CreateMatchFixturesCommandResult> Handle(CreateMatchFixturesCommand request, CancellationToken cancellationToken)
        {
            if (request.MatchType == "R16")
                await CreateR16MatchFixtures(request);
            if (request.MatchType == "R32")
                await CreateR32MatchFixtures(request.TeamsSeedDetails);
            if (request.MatchType == "R64")
                await CreateR64MatchFixtures(request.TeamsSeedDetails);

            return new CreateMatchFixturesCommandResult { IsSuccess = true };
        }
        private async Task CreateR16MatchFixtures(CreateMatchFixturesCommand request)
        {

            if (request.TeamsSeedDetails.Count != 16)
                throw new ArgumentException("The number of teams must be 16.");


            var seededTeams = request.TeamsSeedDetails.OrderByDescending(SeedDetails => SeedDetails.Value).ToList();

            var groupStageRunnerUpTeams = seededTeams.Take(8).ToList();
            var groupStageWinnerTeams = seededTeams.Skip(8).ToList();


            var roundOf16Matches = new List<Match>();

            for (int j = 7; j >= 0; j--)
            {
                var match = new Match();
                if (j % 2 == 1)
                {
                    match.TournamentID = request.TournamentID;
                    match.MatchID = Guid.NewGuid();
                    match.HomeTeamID = groupStageWinnerTeams[j].Key;
                    match.AwayTeamID = groupStageRunnerUpTeams[j - 1].Key;
                }

                else
                {
                    match.TournamentID = request.TournamentID;
                    match.MatchID = Guid.NewGuid();
                    match.HomeTeamID = groupStageWinnerTeams[j].Key;
                    match.AwayTeamID = groupStageRunnerUpTeams[j + 1].Key;
                }
                roundOf16Matches.Add(match);
            }

            matchRepository.Matches.AddRange(roundOf16Matches);

        }
        private async Task CreateR32MatchFixtures(Dictionary<Guid, string> SeedDetails)
        {
            return;
        }
        private async Task CreateR64MatchFixtures(Dictionary<Guid, string> SeedDetails)
        {
            return;
        }

        private async Task CreateTeamMatchMaps(CreateMatchFixturesCommand request)
        {

        }
    }

}
