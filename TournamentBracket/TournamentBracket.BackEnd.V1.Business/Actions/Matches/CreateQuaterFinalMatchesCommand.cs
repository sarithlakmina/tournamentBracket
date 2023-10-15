using MediatR;
using TournamentBracket.BackEnd.V1.Business.Actions.Definitions;
using TournamentBracket.BackEnd.V1.Common.Constants;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Business.Actions.Matches;

public class CreateQuaterFinalMatchesCommand : IRequest<CreateQuaterFinalMatchesCommandResult>
{
    public Guid TournamentID { get; set; }
    public List<string> Seeds { get; set; }
    public Dictionary<Guid, string> TeamIDSeedMap { get; set; }
}

public class CreateQuaterFinalMatchesCommandResult
{ }

public class CreateQuaterFinalMatchesCommandHandler : BackEndGenericHandler, IRequestHandler<CreateQuaterFinalMatchesCommand, CreateQuaterFinalMatchesCommandResult>
{
    public CreateQuaterFinalMatchesCommandHandler(ITournamentBracketDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<CreateQuaterFinalMatchesCommandResult> Handle(CreateQuaterFinalMatchesCommand request, CancellationToken cancellationToken)
    {
        var orderedSeeds = request.Seeds.OrderBy(s => s).ToList();

        var possibleQuaterFinalGroup1 = BracketLists.QuaterFinalGroup1;
        var possibleQuaterFinalGroup2 = BracketLists.QuaterFinalGroup2;

        var quaterFinalsGroupA = new List<string>();
        var quaterFinalsGroupB = new List<string>();


        var quaterFinalGroupAPossibleMatches = new List<string>();

        foreach (string item in orderedSeeds)
        {
            if (possibleQuaterFinalGroup1.Contains(item))
            {
                quaterFinalsGroupA.Add(item);
            }
            else
            {
                quaterFinalsGroupB.Add(item);
            }
        }

        var quaterFinalMatches = new List<Match>();
        var quaterFinalMatchMatchCategoryMaps = new List<MatchMatchCategoryMap>();
        var tournamentMatchMaps = new List<TournamentMatchMap>();

        var dictionaryOfPossibleMatchesCombination = new Dictionary<string, List<string>>();
        var possibleAwayTeams = new List<string>();

        #region Add Match Category

        var matchcategoryID = Guid.NewGuid();
        var matchCategory = new MatchCategory
        {
            MatchCategoryID = matchcategoryID,
            Name = MatchCategoryType.QuaterFinals,
            TournamentID = request.TournamentID,
        };

        matchCategoryRepository.MatchCategories.Add(matchCategory);

        #endregion


        for (int i = 0; i <= 7; i++)
        {
            #region Add Matches

            if (i < 1)
            {
                var quaterFinalGroup1HomeTeam1 = possibleQuaterFinalGroup1[i];
                var quaterFinalGroup1HomeTeam2 = possibleQuaterFinalGroup1[i + 1];

                var quaterFinalGroup1AwayTeam1 = possibleQuaterFinalGroup1[i + 2];
                var quaterFinalGroup1AwayTeam2 = possibleQuaterFinalGroup1[i + 3];

                var quaterFinalGroup2HomeTeam1 = possibleQuaterFinalGroup2[i];
                var quaterFinalGroup2HomeTeam2 = possibleQuaterFinalGroup2[i + 1];

                var quaterFinalGroup2AwayTeam1 = possibleQuaterFinalGroup2[i + 2];
                var quaterFinalGroup2AwayTeam2 = possibleQuaterFinalGroup2[i + 3];

                var possibleAwayTeamsGroup1 = new List<string> { quaterFinalGroup1AwayTeam1, quaterFinalGroup1AwayTeam2 };
                var possibleAwayTeamsGroup2 = new List<string> { quaterFinalGroup2AwayTeam1, quaterFinalGroup2AwayTeam2 };

                dictionaryOfPossibleMatchesCombination = new Dictionary<string, List<string>>
                {
                    { quaterFinalGroup1HomeTeam1, possibleAwayTeamsGroup1 },
                    { quaterFinalGroup1HomeTeam2, possibleAwayTeamsGroup1 },

                    { quaterFinalGroup2HomeTeam1, possibleAwayTeamsGroup2 },
                    { quaterFinalGroup2HomeTeam2, possibleAwayTeamsGroup2 }
                };

                i += 3;   //to skip other team validation

            }
            else
            {
                var quaterFinalGroup3HomeTeam1 = possibleQuaterFinalGroup1[i];
                var quaterFinalGroup3HomeTeam2 = possibleQuaterFinalGroup1[i + 1];

                var quaterFinalGroup3AwayTeam1 = possibleQuaterFinalGroup1[i + 2];
                var quaterFinalGroup3AwayTeam2 = possibleQuaterFinalGroup1[i + 3];

                var quaterFinalGroup4HomeTeam1 = possibleQuaterFinalGroup2[i];
                var quaterFinalGroup4HomeTeam2 = possibleQuaterFinalGroup2[i + 1];

                var quaterFinalGroup4AwayTeam1 = possibleQuaterFinalGroup2[i + 2];
                var quaterFinalGroup4AwayTeam2 = possibleQuaterFinalGroup2[i + 3];

                var possibleAwayTeamsG3Group1 = new List<string> { quaterFinalGroup3AwayTeam1, quaterFinalGroup3AwayTeam2 };
                var possibleAwayTeamsG3Group2 = new List<string> { quaterFinalGroup4AwayTeam1, quaterFinalGroup4AwayTeam2 };

                dictionaryOfPossibleMatchesCombination = new Dictionary<string, List<string>>
                {
                    { quaterFinalGroup3HomeTeam1, possibleAwayTeamsG3Group1 },
                    { quaterFinalGroup3HomeTeam2, possibleAwayTeamsG3Group1 },

                    { quaterFinalGroup4HomeTeam1, possibleAwayTeamsG3Group2 },
                    { quaterFinalGroup4HomeTeam2, possibleAwayTeamsG3Group2 }
                };

                i = 7;
            }
            quaterFinalMatches.AddRange(createQuaterFinalMatches(request.Seeds, request.TeamIDSeedMap, dictionaryOfPossibleMatchesCombination, request.TournamentID));

            #endregion

        }

        foreach (var item in quaterFinalMatches)
        {
            #region Add Match Match Category Maps
            var matchMatchCategoryMap = new MatchMatchCategoryMap
            {
                MatchMatchCategoryMapID = Guid.NewGuid(),
                MatchCategoryID = matchcategoryID,
                MatchID = item.MatchID,
                TournamentID = request.TournamentID,
                IsMatchCompleted = false
            };

            quaterFinalMatchMatchCategoryMaps.Add(matchMatchCategoryMap);

            #endregion

            #region Add Tournament Match  Maps
            var tournamentMatchMap = new TournamentMatchMap
            {
                TournamentMatchMapID = Guid.NewGuid(),
                MatchID = item.MatchID,
                CreatedAt = DateTimeOffset.Now,
                TournamentID = request.TournamentID,
            };

            tournamentMatchMaps.Add(tournamentMatchMap);
            #endregion
        }

        matchRepository.Matches.AddRange(quaterFinalMatches);
        matchMatchCategoryMapRepository.MatchMatchCategoryMaps.AddRange(quaterFinalMatchMatchCategoryMaps);
        tournamentMatchMapRepository.TournamentMatchMaps.AddRange(tournamentMatchMaps);

        await unitOfWork.SaveChangesAsync();

        return new CreateQuaterFinalMatchesCommandResult();
    }
    private List<Match> createQuaterFinalMatches(List<string> Seeds, Dictionary<Guid, string> teamIDSeedMap, Dictionary<string, List<string>> k, Guid tournamentID)
    {
        var matchList = new List<Match>();

        foreach (var item in k)
        {
            var match = new Match
            {
                TournamentID = tournamentID
            };

            if (Seeds.Contains(item.Key))
            {
                match.HomeTeamID = teamIDSeedMap.FirstOrDefault(x => x.Value == item.Key).Key;

                foreach (var item1 in item.Value)
                {
                    if (Seeds.Contains(item1))
                    {
                        match.AwayTeamID = teamIDSeedMap.FirstOrDefault(x => x.Value == item1).Key;
                        match.MatchID = Guid.NewGuid();
                        matchList.Add(match);
                    }
                }
            }

        }

        return matchList;

    }
}
