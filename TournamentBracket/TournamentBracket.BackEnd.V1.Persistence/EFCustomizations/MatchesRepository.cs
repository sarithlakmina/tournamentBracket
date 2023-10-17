using Microsoft.EntityFrameworkCore;
using System.Data;
using TournamentBracket.BackEnd.V1.Common.Entity;
using TournamentBracket.BackEnd.V1.Common.Repository;

namespace TournamentBracket.BackEnd.V1.Persistence.EFCustomizations;

public partial class TournamentBracketDbContext : IMatchRepository
{
    public Task<List<Match>> GetAllMatches()
        => Matches.ToListAsync();

    public Task<List<Match>> GetAllMatches(List<Guid> TeamIDs)
        => Matches
        .Where(m => !m.IsMatchCompleted && (TeamIDs.Contains(m.HomeTeamID) || TeamIDs.Contains(m.AwayTeamID)))
        .ToListAsync();

    public Task<List<Match>> GetAllMatchesByMatchIDs(List<Guid> MatchIDs)
        => Matches
        .Where(m => !m.IsMatchCompleted && (MatchIDs.Contains(m.MatchID)))
        .ToListAsync();

    public Task<Match> GetCurrentMatchByTeam(Guid teamID)
    => Matches.Where(m => (m.HomeTeamID == teamID || m.AwayTeamID == teamID) && !m.IsMatchCompleted)
        .FirstOrDefaultAsync();
}
