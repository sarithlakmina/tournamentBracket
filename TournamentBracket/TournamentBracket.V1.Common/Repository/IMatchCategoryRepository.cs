using Microsoft.EntityFrameworkCore;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Common.Repository;

public interface IMatchCategoryRepository : IUnitOfWork
{
    public DbSet<MatchCategory> MatchCategories { get; set; }
    public Task<List<MatchCategory>> GetAllMatchCategories();
}
