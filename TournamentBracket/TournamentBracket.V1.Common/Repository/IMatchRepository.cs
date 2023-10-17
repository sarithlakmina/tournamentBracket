using Microsoft.EntityFrameworkCore;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Common.Repository;

public interface IMatchRepository : IUnitOfWork
{
    public DbSet<Match> Matches { get; set; }
    public Task<List<Match>> GetAllMatches();
    public Task<List<Match>> GetAllMatches(List<Guid> TeamIDs);
    public Task<List<Match>> GetAllMatchesByMatchIDs(List<Guid> MatchIDs);
    public Task<Match> GetCurrentMatchByTeam(Guid teamID);
}
