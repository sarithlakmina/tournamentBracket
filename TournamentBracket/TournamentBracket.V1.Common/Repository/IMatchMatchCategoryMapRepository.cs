using Microsoft.EntityFrameworkCore;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Common.Repository;

public interface IMatchMatchCategoryMapRepository : IUnitOfWork
{
    DbSet<MatchMatchCategoryMap> MatchMatchCategoryMaps { get; set; }

    Task<List<MatchMatchCategoryMap>> GetMatchMatchCategoryMap(Guid TournamentID, Guid MatchID);
    Task<List<MatchMatchCategoryMap>> GetMatchMatchCategoryMapByCategory(Guid TournamentID, Guid MatchCategoryID);
}
