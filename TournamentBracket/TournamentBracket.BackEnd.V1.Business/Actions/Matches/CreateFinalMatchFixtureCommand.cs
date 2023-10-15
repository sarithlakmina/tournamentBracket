using MediatR;
using TournamentBracket.BackEnd.V1.Business.Actions.Definitions;
using TournamentBracket.BackEnd.V1.Common.Constants;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Business.Actions.Matches;

public class CreateFinalMatchFixtureCommand : IRequest<CreateFinalMatchFixtureResult>
{
    public Guid TournamentID { get; set; }
}

public class CreateFinalMatchFixtureResult
{ }

public class CreateFinalMatchFixtureHandler : BackEndGenericHandler, IRequestHandler<CreateFinalMatchFixtureCommand, CreateFinalMatchFixtureResult>
{
    public CreateFinalMatchFixtureHandler(ITournamentBracketDbContext dbContext) : base(dbContext) { }

    public async Task<CreateFinalMatchFixtureResult> Handle(CreateFinalMatchFixtureCommand request, CancellationToken cancellationToken)
    {
        var matchFixture = await matchCategoryRepository.GetMatchCategory(request.TournamentID, MatchCategoryType.SemiFinals);

        if (matchFixture == null)
            throw new Exception(ExceptionMessages.MatchNotFoundException);

        var matchCategoryID = matchFixture.Select(x => x.MatchCategoryID).FirstOrDefault();

        var matchMatchCategoryMaps = await matchMatchCategoryMapRepository.GetMatchMatchCategoryMapByCategory(request.TournamentID, matchCategoryID);

        if (matchMatchCategoryMaps == null)
            throw new Exception(ExceptionMessages.MatchNotFoundException);

        var semiFinalMatches = new List<Guid>();
        var finalMatchTeams = new List<Guid>();

        foreach (var item in matchMatchCategoryMaps)
        {
            semiFinalMatches.Add(item.MatchID);
        }

        var matchList = await matchRepository.GetAllMatchesByMatchIDs(semiFinalMatches);

        if (matchList == null)
            throw new Exception(ExceptionMessages.MatchNotFoundException);

        #region Add Match Category - Semi finals

        var matchcategoryID = Guid.NewGuid();
        var matchCategory = new MatchCategory
        {
            MatchCategoryID = matchcategoryID,
            Name = MatchCategoryType.Finals,
            TournamentID = request.TournamentID,
        };

        matchCategoryRepository.MatchCategories.Add(matchCategory);
        #endregion

        #region Add final Match

        foreach (var item in matchList)
        {
            finalMatchTeams.Add(item.WinningTeamID.Value);
        }

        var finalMatchID = Guid.NewGuid();
        var homeTeamID = matchList[0].WinningTeamID.Value;
        var awayTeamID = matchList[1].WinningTeamID.Value;

        var finalMatch = new Match
        {
            TournamentID = request.TournamentID,
            MatchID = finalMatchID,
            HomeTeamID = homeTeamID,
            AwayTeamID = awayTeamID,
            IsMatchCompleted = false,
        };

        matchRepository.Matches.Add(finalMatch);
        #endregion

        #region Add Match Match Category Maps
        var matchMatchCategoryMap = new MatchMatchCategoryMap
        {
            MatchMatchCategoryMapID = Guid.NewGuid(),
            MatchCategoryID = matchcategoryID,
            MatchID = finalMatchID,
            TournamentID = request.TournamentID,
            IsMatchCompleted = false
        };

        matchMatchCategoryMapRepository.MatchMatchCategoryMaps.Add(matchMatchCategoryMap);

        #endregion

        #region Add Tournament Match  Maps
        var tournamentMatchMap = new TournamentMatchMap
        {
            TournamentMatchMapID = Guid.NewGuid(),
            MatchID = finalMatchID,
            CreatedAt = DateTimeOffset.Now,
            TournamentID = request.TournamentID,
        };

        tournamentMatchMapRepository.TournamentMatchMaps.Add(tournamentMatchMap);
        #endregion

        await unitOfWork.SaveChangesAsync();

        return new CreateFinalMatchFixtureResult();
    }
}

