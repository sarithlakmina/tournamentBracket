﻿using MediatR;
using TournamentBracket.BackEnd.V1.Business.Actions.Definitions;
using TournamentBracket.BackEnd.V1.Common.Constants;
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
            if (request.MatchType == MatchCategoryType.RoundOf16)
                await CreateR16MatchFixtures(request);
            if (request.MatchType == MatchCategoryType.GroupStage)
                await CreateGroupStageMatchFixtures(request.TeamsSeedDetails);
            if (request.MatchType == MatchCategoryType.RoundOf64)
                await CreateR64MatchFixtures(request.TeamsSeedDetails);

            return new CreateMatchFixturesCommandResult { IsSuccess = true };
        }
        private async Task CreateR16MatchFixtures(CreateMatchFixturesCommand request)
        {

            if (request.TeamsSeedDetails.Count != 16)
                throw new ArgumentException(ExceptionMessages.NumberOfTeamsIncorrectException);


            var seededTeams = request.TeamsSeedDetails.OrderByDescending(SeedDetails => SeedDetails.Value).ToList();

            var groupStageRunnerUpTeams = seededTeams.Take(8).ToList();
            var groupStageWinnerTeams = seededTeams.Skip(8).ToList();


            var roundOf16Matches = new List<Match>();
            var roundOf16MatchMatchCategoryMaps = new List<MatchMatchCategoryMap>();
            var tournamentMatchMaps = new List<TournamentMatchMap>();

            #region Add Match Category

            var matchcategoryID = Guid.NewGuid();
            var matchCategory = new Common.Entity.MatchCategory
            {
                MatchCategoryID = matchcategoryID,
                MatchTypeName = MatchCategoryType.RoundOf16,
                TournamentID = request.TournamentID,
            };

            matchCategoryRepository.MatchCategories.Add(matchCategory);
            #endregion

            #region Add Matches and Match Match Category Maps
            for (int j = 7; j >= 0; j--)
            {
                #region Add Matches
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
                #endregion

                #region Add Match Match Category Maps
                var matchMatchCategoryMap = new MatchMatchCategoryMap
                {
                    MatchMatchCategoryMapID = Guid.NewGuid(),
                    MatchCategoryID = matchcategoryID,
                    MatchID = match.MatchID,
                    TournamentID = request.TournamentID,
                    IsMatchCompleted = false
                };

                roundOf16MatchMatchCategoryMaps.Add(matchMatchCategoryMap);

                #endregion

                #region Add Tournament Match  Maps
                var tournamentMatchMap = new TournamentMatchMap
                {
                    TournamentMatchMapID = Guid.NewGuid(),
                    MatchID = match.MatchID,
                    CreatedAt = DateTimeOffset.Now,
                    IsMatchCompleted = false,
                    TournamentID = request.TournamentID,
                };

                tournamentMatchMaps.Add(tournamentMatchMap);
                #endregion
            }

            matchRepository.Matches.AddRange(roundOf16Matches);
            MatchMatchCategoryMapRepository.MatchMatchCategoryMaps.AddRange(roundOf16MatchMatchCategoryMaps);
            tournamentMatchMapRepository.TournamentMatchMaps.AddRange(tournamentMatchMaps);

            #endregion

            await CreateQuaterFinalFixtures(request);

        }
        private async Task CreateGroupStageMatchFixtures(Dictionary<Guid, string> SeedDetails)
        {
            return;
        }
        private async Task CreateR64MatchFixtures(Dictionary<Guid, string> SeedDetails)
        {
            return;
        }

        private async Task CreateQuaterFinalFixtures(CreateMatchFixturesCommand request)
        {


            request.TournamentID = Guid.Parse("1DBA19A1-A8A8-4E33-BEBD-215CDC9CA1F8");

            var winners = await matchRepository.GetMatchWinners(request.TournamentID, MatchCategoryType.RoundOf16);

            if (winners.Count != 8)
                throw new ArgumentException(ExceptionMessages.NumberOfTeamsIncorrectException);



            var quarterFinalMatches = new List<Match>();

            for (int k = 0; k < 4; k++)
            {
                var match = new Match
                {
                    TournamentID = request.TournamentID,
                    MatchID = Guid.NewGuid(),
                    HomeTeamID = winners[2 * k].TeamID, // Assuming you have a WinnerTeamID property in your Match class
                    AwayTeamID = winners[2 * k + 1].TeamID
                };
                var c = winners[2 * k].Seed;
                var d = winners[2 * k + 1].Seed;
                quarterFinalMatches.Add(match);
            }

            matchRepository.Matches.AddRange(quarterFinalMatches);
        }
    }

}
