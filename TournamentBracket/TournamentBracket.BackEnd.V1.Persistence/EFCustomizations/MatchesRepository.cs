﻿using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TournamentBracket.BackEnd.V1.Common.Constants;
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

    public async Task<List<Team>> GetMatchWinners(Guid TournamentID, string MatchCategoryName)
    {
        var connection = Database.GetDbConnection();

        var parameters = new DynamicParameters();

        parameters.Add(CommonSPParamNames.TournamentID, TournamentID);
        parameters.Add(CommonSPParamNames.MatchCategoryName, MatchCategoryName);

        var winners = await connection.QueryAsync<Team>(StoredProcedureNames.GetMatchWinnersData, param: parameters, commandType: CommandType.StoredProcedure);

        return winners.ToList();
    }
}
