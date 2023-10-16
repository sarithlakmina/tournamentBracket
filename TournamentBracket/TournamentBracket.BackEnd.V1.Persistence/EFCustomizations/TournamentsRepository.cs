using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TournamentBracket.BackEnd.V1.Common.Constants;
using TournamentBracket.BackEnd.V1.Common.DTO;
using TournamentBracket.BackEnd.V1.Common.Entity;
using TournamentBracket.BackEnd.V1.Common.Repository;

namespace TournamentBracket.BackEnd.V1.Persistence.EFCustomizations;

public partial class TournamentBracketDbContext : ITournamentRepository
{
    public Task<bool> DoesTournamentExist(Guid TournamentID)
          => Tournaments.AnyAsync(o => o.TournamentID == TournamentID);
    public Task<List<Tournament>> GetAllTournaments()
          => Tournaments.ToListAsync();

    public async Task<List<PathToVictoryDto>> GetPathToVictory(Guid TournamentID, Guid TeamID)
    {
        var connection = Database.GetDbConnection();

        var parameters = new DynamicParameters();

        parameters.Add(CommonSPParamNames.TournamentID, TournamentID);
        parameters.Add(CommonSPParamNames.TeamID, TeamID);

        var winners = await connection.QueryAsync<PathToVictoryDto>(StoredProcedureNames.GetPathToVictory, param: parameters, commandType: CommandType.StoredProcedure);

        return winners.ToList();
    }

    public Task<Tournament> GetTournament(Guid TournamentID)
           => Tournaments.FirstOrDefaultAsync(t => t.TournamentID == TournamentID);



    public Task<List<string>> GetTournamentWinner(Guid tournamentID)
        => Teams.Join(Tournaments,
                tm => tm.TeamID,
                t => t.Winner,
                (tm, t) => new { Team = tm, Tournament = t })
                .Where(joinResult => joinResult.Tournament.TournamentID == tournamentID)
                .Select(joinResult => joinResult.Team.Name).ToListAsync();

    public Task<Guid?> GetTournamentWinnerIDs(Guid TournamentID)
        => Tournaments.Where(t => t.TournamentID == TournamentID && t.Winner.HasValue).Select(t => t.Winner).FirstAsync();
}
