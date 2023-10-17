using Microsoft.EntityFrameworkCore;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.DTO;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Common.Repository;

public interface ITournamentRepository : IUnitOfWork
{
    public DbSet<Tournament> Tournaments { get; set; }
    public Task<List<Tournament>> GetAllTournaments();
    public Task<Tournament> GetTournament(Guid TournamentID);
    public Task<List<PathToVictoryDto>> GetPathToVictory(Guid TournamentID, Guid WinningTeamID);
    public Task<List<string>> GetTournamentWinner(Guid TournamentID);
    public Task<Guid?> GetTournamentWinnerIDs(Guid TournamentID);
    public Task<bool> IsTournamentNameUniqueToCreate(string Name);
}


