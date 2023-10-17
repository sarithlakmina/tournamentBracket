using Microsoft.EntityFrameworkCore;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Common.Repository;
public interface ITournamentMatchMapRepository : IUnitOfWork
{
    public DbSet<TournamentMatchMap> TournamentMatchMaps { get; set; }

    public Task<List<TournamentMatchMap>> GetTournamentMatchMap(Guid TournamentID);
}
