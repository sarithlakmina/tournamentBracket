using Microsoft.EntityFrameworkCore;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Persistence.EFCustomizations;

public partial class TournamentBracketDbContext : DbContext
{
    public TournamentBracketDbContext(DbContextOptions<TournamentBracketDbContext> options) : base(options) { }

    protected TournamentBracketDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TournamentBracketDbContext).Assembly);
    }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<MatchCategory> MatchCategories { get; set; }
    public DbSet<TournamentMatchMap> TournamentMatchMaps { get; set; }
    public DbSet<MatchMatchCategoryMap> MatchMatchCategoryMaps { get; set; }
    public DbSet<TournamentTeamMap> TournamentTeamMaps { get; set; }


}
