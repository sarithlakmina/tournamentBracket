using Microsoft.EntityFrameworkCore;
using TournamentBracket.BackEnd.V1.Common.Entity;
using TournamentBracket.BackEnd.V1.Common.Repository;

namespace TournamentBracket.BackEnd.V1.Persistence.EFCustomizations;

public partial class TournamentBracketDbContext : IMatchCategoryRepository
{
    public Task<List<MatchCategory>> GetAllMatchCategories()
    => MatchCategories.ToListAsync();

}
