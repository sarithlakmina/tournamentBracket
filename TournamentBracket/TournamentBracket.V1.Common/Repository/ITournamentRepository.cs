using Microsoft.EntityFrameworkCore;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Common.Repository;

public interface ITournamentRepository : IUnitOfWork
{
    public DbSet<Tournament> Tournaments { get; set; }
    public Task<List<Tournament>> GetAllTournaments();
}
