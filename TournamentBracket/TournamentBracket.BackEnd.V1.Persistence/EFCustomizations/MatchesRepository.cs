using Microsoft.EntityFrameworkCore;
using TournamentBracket.BackEnd.V1.Common.DTO;
using TournamentBracket.BackEnd.V1.Common.Entity;
using TournamentBracket.BackEnd.V1.Common.Repository;

namespace TournamentBracket.BackEnd.V1.Persistence.EFCustomizations;

public partial class TournamentBracketDbContext : IMatchRepository
{
    public Task<List<Match>> GetAllMatches()
        => Matches.ToListAsync();

    public Task<Match> GetCurrentMatchByTeam(Guid teamID)
    => Matches.Where(m => (m.HomeTeamID == teamID || m.AwayTeamID == teamID) && !m.IsMatchCompleted)
        .FirstOrDefaultAsync();

    public Task<MatchDto> GetMatchWinners(Guid TournamentID, Guid MatchID, Guid TeamID)
    {
        throw new NotImplementedException();
    }
}
