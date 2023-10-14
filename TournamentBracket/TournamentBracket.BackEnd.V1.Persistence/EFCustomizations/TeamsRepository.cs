using Microsoft.EntityFrameworkCore;
using TournamentBracket.BackEnd.V1.Common.DTO;
using TournamentBracket.BackEnd.V1.Common.Entity;
using TournamentBracket.BackEnd.V1.Common.Repository;

namespace TournamentBracket.BackEnd.V1.Persistence.EFCustomizations;

public partial class TournamentBracketDbContext : ITeamRepository
{
    public Task<List<Team>> GetAllTeams()
        => Teams.ToListAsync();

    public Task<TeamDto> GetTeamByName(string teamName)
        => Teams.Where(t => t.Name.Equals(teamName)).Select(td => new TeamDto
        {
            Name = td.Name,
            TeamID = td.TeamID,
            Seed = td.Seed,
        }).FirstOrDefaultAsync();
}
