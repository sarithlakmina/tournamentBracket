using Microsoft.EntityFrameworkCore;
using TournamentBracket.BackEnd.V1.Common.Entity;
using TournamentBracket.BackEnd.V1.Common.Repository;

namespace TournamentBracket.BackEnd.V1.Persistence.EFCustomizations;

public partial class TournamentBracketDbContext : IMatchMatchCategoryMapRepository
{
    public Task<List<MatchMatchCategoryMap>> GetMatchMatchCategoryMap(Guid TournamentID, Guid MatchID)
    => MatchMatchCategoryMaps.Where(mmtm => mmtm.TournamentID == TournamentID && mmtm.MatchID == MatchID).ToListAsync();

    public Task<List<MatchMatchCategoryMap>> GetMatchMatchCategoryMapByCategory(Guid TournamentID, Guid MatchCategoryID)
        => MatchMatchCategoryMaps.Where(mmtm => mmtm.TournamentID == TournamentID && mmtm.MatchCategoryID == MatchCategoryID).ToListAsync();
}
