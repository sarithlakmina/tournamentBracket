using Microsoft.EntityFrameworkCore;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.DTO;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Common.Repository;

public interface ITeamRepository : IUnitOfWork
{
    public DbSet<Team> Teams { get; set; }

    public Task<List<Team>> GetAllTeams();
    public Task<TeamDto> GetTeamByName(string teamName);
}
