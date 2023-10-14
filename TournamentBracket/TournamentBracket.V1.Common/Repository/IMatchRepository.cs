using Microsoft.EntityFrameworkCore;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.DTO;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Common.Repository;

public interface IMatchRepository : IUnitOfWork
{
    public DbSet<Match> Matches { get; set; }
    public Task<List<Match>> GetAllMatches();
    public Task<List<Match>> GetAllMatches(List<Guid> TeamIDs);
    public Task<Match> GetCurrentMatchByTeam(Guid teamID);
    public Task<List<WinnerDto>> GetMatchWinners(Guid TournamentID, string MatchCategoryName);
}
