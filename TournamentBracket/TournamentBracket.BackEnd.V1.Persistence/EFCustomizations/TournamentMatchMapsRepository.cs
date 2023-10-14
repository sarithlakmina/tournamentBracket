using Microsoft.EntityFrameworkCore;
using TournamentBracket.BackEnd.V1.Common.Entity;
using TournamentBracket.BackEnd.V1.Common.Repository;

namespace TournamentBracket.BackEnd.V1.Persistence.EFCustomizations;

public partial class TournamentBracketDbContext : ITournamentMatchMapRepository
{
    public Task<List<TournamentMatchMap>> GetTournamentMatchMap(Guid TournamentID)
        => TournamentMatchMaps.Where(tmm => tmm.TournamentMatchMapID == TournamentID).ToListAsync();
}
