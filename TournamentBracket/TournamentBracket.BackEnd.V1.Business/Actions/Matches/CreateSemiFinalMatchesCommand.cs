using MediatR;
using TournamentBracket.BackEnd.V1.Business.Actions.Definitions;
using TournamentBracket.BackEnd.V1.Common.Constants;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Business.Actions.Matches
{

    public class CreateSemiFinalMatchesCommand : IRequest<CreateSemiFinalMatchesCommandResult>
    {
        public Guid TournamentID { get; set; }
        public List<string> Seeds { get; set; }
        public Dictionary<Guid, string> TeamIDSeedMap { get; set; }
    }

    public class CreateSemiFinalMatchesCommandResult
    { }

    public class CreateSemiFinalMatchesCommandHandler : BackEndGenericHandler, IRequestHandler<CreateSemiFinalMatchesCommand, CreateSemiFinalMatchesCommandResult>
    {
        public CreateSemiFinalMatchesCommandHandler(ITournamentBracketDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<CreateSemiFinalMatchesCommandResult> Handle(CreateSemiFinalMatchesCommand request, CancellationToken cancellationToken)
        {
            var orderedSeeds = request.Seeds.OrderBy(s => s).ToList();

            var firstSemiFinalMatchSeeds = new List<string>();

            var quaterFinalMatchMatchCategoryMaps = new List<MatchMatchCategoryMap>();
            var tournamentMatchMaps = new List<TournamentMatchMap>();

            foreach (var seedFromFixture in BracketLists.QuaterFinalGroup1)
            {
                foreach (var seedFromWinning in request.Seeds)
                {
                    if (seedFromFixture.Equals(seedFromWinning))
                        firstSemiFinalMatchSeeds.Add(seedFromWinning);
                }
            }

            var matchList = new List<Match>();

            var secondSemiFinalMatchSeeds = request.Seeds.Except(firstSemiFinalMatchSeeds).ToList();

            var firstSemiFinalMatchID = Guid.NewGuid();
            var secondSemiFinalMatchID = Guid.NewGuid();


            var match1 = new Match
            {
                TournamentID = request.TournamentID,
                MatchID = firstSemiFinalMatchID
            };

            match1.HomeTeamID = request.TeamIDSeedMap.FirstOrDefault(x => x.Value == firstSemiFinalMatchSeeds[0]).Key;
            match1.AwayTeamID = request.TeamIDSeedMap.FirstOrDefault(x => x.Value == firstSemiFinalMatchSeeds[1]).Key;

            matchList.Add(match1);

            var match2 = new Match
            {
                TournamentID = request.TournamentID,
                MatchID = secondSemiFinalMatchID
            };

            match2.HomeTeamID = request.TeamIDSeedMap.FirstOrDefault(x => x.Value == secondSemiFinalMatchSeeds[0]).Key;
            match2.AwayTeamID = request.TeamIDSeedMap.FirstOrDefault(x => x.Value == secondSemiFinalMatchSeeds[1]).Key;

            matchList.Add(match2);

            #region Add Match Category - Semi finals

            var matchcategoryID = Guid.NewGuid();
            var matchCategory = new MatchCategory
            {
                MatchCategoryID = matchcategoryID,
                Name = MatchCategoryType.SemiFinals,
                TournamentID = request.TournamentID,
            };

            matchCategoryRepository.MatchCategories.Add(matchCategory);

            #endregion

            #region Add Match Match Category Maps
            var matchMatchCategoryMap1 = new MatchMatchCategoryMap
            {
                MatchMatchCategoryMapID = Guid.NewGuid(),
                MatchCategoryID = matchcategoryID,
                MatchID = firstSemiFinalMatchID,
                TournamentID = request.TournamentID,
                IsMatchCompleted = false
            };

            quaterFinalMatchMatchCategoryMaps.Add(matchMatchCategoryMap1);

            var matchMatchCategoryMap2 = new MatchMatchCategoryMap
            {
                MatchMatchCategoryMapID = Guid.NewGuid(),
                MatchCategoryID = matchcategoryID,
                MatchID = secondSemiFinalMatchID,
                TournamentID = request.TournamentID,
                IsMatchCompleted = false
            };

            quaterFinalMatchMatchCategoryMaps.Add(matchMatchCategoryMap2);

            #endregion

            #region Add Tournament Match  Maps
            var tournamentMatchMap1 = new TournamentMatchMap
            {
                TournamentMatchMapID = Guid.NewGuid(),
                MatchID = firstSemiFinalMatchID,
                CreatedAt = DateTimeOffset.Now,
                TournamentID = request.TournamentID,
            };

            tournamentMatchMaps.Add(tournamentMatchMap1);

            var tournamentMatchMap2 = new TournamentMatchMap
            {
                TournamentMatchMapID = Guid.NewGuid(),
                MatchID = secondSemiFinalMatchID,
                CreatedAt = DateTimeOffset.Now,
                TournamentID = request.TournamentID,
            };

            tournamentMatchMaps.Add(tournamentMatchMap2);
            #endregion

            matchRepository.Matches.AddRange(matchList);
            MatchMatchCategoryMapRepository.MatchMatchCategoryMaps.AddRange(quaterFinalMatchMatchCategoryMaps);
            tournamentMatchMapRepository.TournamentMatchMaps.AddRange(tournamentMatchMaps);

            await unitOfWork.SaveChangesAsync();

            return new CreateSemiFinalMatchesCommandResult();
        }
    }

}
