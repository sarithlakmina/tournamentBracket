using MediatR;
using TournamentBracket.BackEnd.V1.Business.Actions.Definitions;
using TournamentBracket.BackEnd.V1.Common.Constants;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Entity;
using TournamentBracket.BackEnd.V1.Common.Model;

namespace TournamentBracket.BackEnd.V1.Business.Actions.Teams
{

    public class AdvanceTeamCommand : IRequest<AdvanceTeamCommandResult>
    {
        public AdvanceTeamRequestModel AdvanceTeamRequest { get; set; }
    }

    public class AdvanceTeamCommandResult
    { }

    public class AdvanceTeamCommandHandler : BackEndGenericHandler, IRequestHandler<AdvanceTeamCommand, AdvanceTeamCommandResult>
    {
        public AdvanceTeamCommandHandler(ITournamentBracketDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<AdvanceTeamCommandResult> Handle(AdvanceTeamCommand request, CancellationToken cancellationToken)
        {
            #region Initiate win for R16 matches
            var roundOf16WinningTeamsTeamIDs = new List<Guid>();

            var teams = await teamRepository.GetAllTeams() ?? throw new Exception(ExceptionMessages.TeamNotFoundException);

            var teamIDSeedMap = teams.ToDictionary(team => team.TeamID, team => team.Seed);

            var roundOf16WinningTeamsSeedList = new List<string>();


            var roundOf16Winners = new HashSet<string>(request.AdvanceTeamRequest.Events);
            roundOf16WinningTeamsTeamIDs.AddRange(teams.Where(team => roundOf16Winners.Contains(team.Name)).Select(team => team.TeamID));

            foreach (var team in roundOf16WinningTeamsTeamIDs)
            {
                roundOf16WinningTeamsSeedList.Add(teamIDSeedMap[team]);
            }

            var quaterFinalmatches = await matchRepository.GetAllMatches(roundOf16WinningTeamsTeamIDs);

            var tournamentID = quaterFinalmatches.Select(m => m.TournamentID).FirstOrDefault();

            if (quaterFinalmatches == null)
                throw new Exception(ExceptionMessages.MatchNotFoundException);

            foreach (var match in quaterFinalmatches)
            {
                match.IsMatchCompleted = true;
                match.WinningTeamID = roundOf16WinningTeamsTeamIDs.Contains(match.AwayTeamID) ? match.AwayTeamID : match.HomeTeamID;
            }


            #endregion


            // Should move to a handler
            #region Create Quater Final fixtures and matches
            await CreateQuaterFinalsMatchFixtures(tournamentID, roundOf16WinningTeamsSeedList, teamIDSeedMap);
            #endregion

            //shoud move to a handler

            #region Initiate win for quater finals

            var quaterFinalsWinningTeamsTeamIDs = new List<Guid>();
            var quaterFinalsWinningTeamsSeedList = new List<string>();

            var quaterFInalWinners = request.AdvanceTeamRequest.Events
               .GroupBy(e => e) // Group the elements by their value
               .Where(el => el.Count() > 1) // Filter groups with count more than 1
               .Select(el => el.Key);


            quaterFinalsWinningTeamsTeamIDs.AddRange(teams.Where(team => quaterFInalWinners.Contains(team.Name)).Select(team => team.TeamID));

            foreach (var team in quaterFinalsWinningTeamsTeamIDs)
            {
                quaterFinalsWinningTeamsSeedList.Add(teamIDSeedMap[team]);
            }

            var matches = await matchRepository.GetAllMatches(quaterFinalsWinningTeamsTeamIDs) ?? throw new Exception(ExceptionMessages.MatchNotFoundException);

            foreach (var match in matches)
            {
                match.IsMatchCompleted = true;
                match.WinningTeamID = quaterFinalsWinningTeamsTeamIDs.Contains(match.AwayTeamID) ? match.AwayTeamID : match.HomeTeamID;
            }
            #endregion

            await CreateSemiFinalsMatchFixtures(tournamentID, quaterFinalsWinningTeamsSeedList, teamIDSeedMap);


            await unitOfWork.SaveChangesAsync();



            return new AdvanceTeamCommandResult();
        }
        #region Semi Finals

        private async Task CreateSemiFinalsMatchFixtures(Guid tournamentID, List<string> Seeds, Dictionary<Guid, string> teamIDSeedMap)
        {
            var orderedSeeds = Seeds.OrderBy(s => s).ToList();

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
                TournamentID = tournamentID,
            };

            matchCategoryRepository.MatchCategories.Add(matchCategory);
            #endregion

            //#region Add Matches and Match Match Category Maps
            for (int i = 0; i <= 3; i++)
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
                quaterFinalMatches.AddRange(createSemiFinalMatches(Seeds, teamIDSeedMap, dictionaryOfPossibleMatchesCombination, tournamentID));

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
                    TournamentID = tournamentID,
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
                    TournamentID = tournamentID,
                };

                tournamentMatchMaps.Add(tournamentMatchMap);
                #endregion
            }

            matchRepository.Matches.AddRange(quaterFinalMatches);
            MatchMatchCategoryMapRepository.MatchMatchCategoryMaps.AddRange(quaterFinalMatchMatchCategoryMaps);
            tournamentMatchMapRepository.TournamentMatchMaps.AddRange(tournamentMatchMaps);

            await unitOfWork.SaveChangesAsync();

        }

        private List<Match> createSemiFinalMatches(List<string> Seeds, Dictionary<Guid, string> teamIDSeedMap, Dictionary<string, List<string>> k, Guid tournamentID)
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
        #endregion
        private async Task CreateQuaterFinalsMatchFixtures(Guid tournamentID, List<string> Seeds, Dictionary<Guid, string> teamIDSeedMap)
        {
            var orderedSeeds = Seeds.OrderBy(s => s).ToList();

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
                TournamentID = tournamentID,
            };

            matchCategoryRepository.MatchCategories.Add(matchCategory);
            #endregion

            //#region Add Matches and Match Match Category Maps
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
                quaterFinalMatches.AddRange(createQuaterFinalMatches(Seeds, teamIDSeedMap, dictionaryOfPossibleMatchesCombination, tournamentID));

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
                    TournamentID = tournamentID,
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
                    TournamentID = tournamentID,
                };

                tournamentMatchMaps.Add(tournamentMatchMap);
                #endregion
            }

            matchRepository.Matches.AddRange(quaterFinalMatches);
            MatchMatchCategoryMapRepository.MatchMatchCategoryMaps.AddRange(quaterFinalMatchMatchCategoryMaps);
            tournamentMatchMapRepository.TournamentMatchMaps.AddRange(tournamentMatchMaps);

            await unitOfWork.SaveChangesAsync();

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

}
