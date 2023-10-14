using Microsoft.EntityFrameworkCore;
using TournamentBracket.BackEnd.V1.Common.Entity;
using TournamentBracket.BackEnd.V1.Common.Repository;

namespace TournamentBracket.BackEnd.V1.Persistence.EFCustomizations;

public partial class TournamentBracketDbContext : IMatchMatchCategoryMapRepository
{
    public Task<List<MatchMatchCategoryMap>> GetMatchMatchTypeMap(Guid TournamentID, Guid MatchID)
    => MatchMatchCategoryMaps.Where(mmtm => mmtm.TournamentID == TournamentID).ToListAsync();
}
