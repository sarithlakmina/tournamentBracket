using Microsoft.EntityFrameworkCore;
using System.Data;
using TournamentBracket.BackEnd.V1.Common.Entity;
using TournamentBracket.BackEnd.V1.Common.Repository;

namespace TournamentBracket.BackEnd.V1.Persistence.EFCustomizations;

public partial class TournamentBracketDbContext : ITournamentRepository
{
    public Task<bool> DoesTournamentExist(Guid TournamentID)
     => Tournaments.AnyAsync(o => o.TournamentID == TournamentID);
    public Task<List<Tournament>> GetAllTournaments()
            => Tournaments.ToListAsync();

    public Task<Tournament> GetTournament(Guid TournamentID)
    {
        return Task.Run(() =>
        {
            return Tournaments.FirstOrDefault(t => t.TournamentID == TournamentID);
        });
    }

    public Task<List<string>> GetTournamentWinner(Guid tournamentID)
    {
        return Task.Run(() =>
        {
            return Teams.Join(Tournaments,
                tm => tm.TeamID,
                t => t.Winner,
                (tm, t) => new { Team = tm, Tournament = t })
                .Where(joinResult => joinResult.Tournament.TournamentID == tournamentID)
                .Select(joinResult => joinResult.Team.Name).ToList();
        });
    }


}
