using TournamentBracket.BackEnd.V1.Common.Repository;

namespace TournamentBracket.BackEnd.V1.Common.Database;

public interface ITournamentBracketDbContext : IUnitOfWork,
    ITeamRepository,
    ITournamentRepository,
    ITournamentTeamMapRepository,
    IMatchRepository,
    ITournamentMatchMapRepository,
    IMatchCategoryRepository,
    IMatchMatchCategoryMapRepository
{ }
