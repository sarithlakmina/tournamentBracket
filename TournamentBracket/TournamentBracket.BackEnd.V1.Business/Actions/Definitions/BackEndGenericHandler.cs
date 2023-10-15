using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Repository;

namespace TournamentBracket.BackEnd.V1.Business.Actions.Definitions;

public class BackEndGenericHandler
{
    private readonly ITournamentBracketDbContext dbContext;

    protected IUnitOfWork unitOfWork;

    protected ITournamentRepository tournamentRepository;
    protected ITeamRepository teamRepository;
    protected IMatchRepository matchRepository;
    protected ITournamentTeamMapRepository tournamentTeamMapRepository;
    protected ITournamentMatchMapRepository tournamentMatchMapRepository;
    protected IMatchCategoryRepository matchCategoryRepository;
    protected IMatchMatchCategoryMapRepository matchMatchCategoryMapRepository;


    public BackEndGenericHandler(ITournamentBracketDbContext dbContext)
    {
        this.dbContext = dbContext;

        unitOfWork = dbContext;

        tournamentRepository = dbContext;
        teamRepository = dbContext;
        matchRepository = dbContext;
        tournamentTeamMapRepository = dbContext;
        tournamentMatchMapRepository = dbContext;
        matchCategoryRepository = dbContext;
        matchMatchCategoryMapRepository = dbContext;
    }
}