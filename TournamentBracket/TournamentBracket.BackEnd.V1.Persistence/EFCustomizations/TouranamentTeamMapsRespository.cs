using Microsoft.EntityFrameworkCore;
using TournamentBracket.BackEnd.V1.Common.Entity;
using TournamentBracket.BackEnd.V1.Common.Repository;

namespace TournamentBracket.BackEnd.V1.Persistence.EFCustomizations;

public partial class TournamentBracketDbContext : ITournamentTeamMapRepository
{
    public Task<TournamentTeamMap> GetTournamentTeamMap(Guid TournamentID)
        => TournamentTeamMaps.Where(ttm => ttm.TournamentID == TournamentID).FirstOrDefaultAsync();
}
