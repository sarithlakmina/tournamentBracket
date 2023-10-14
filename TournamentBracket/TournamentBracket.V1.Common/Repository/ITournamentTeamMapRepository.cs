using Microsoft.EntityFrameworkCore;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Common.Repository;

public interface ITournamentTeamMapRepository : IUnitOfWork
{
    DbSet<TournamentTeamMap> TournamentTeamMaps { get; set; }

    Task<TournamentTeamMap> GetTournamentTeamMap(Guid TournamentID);

}
