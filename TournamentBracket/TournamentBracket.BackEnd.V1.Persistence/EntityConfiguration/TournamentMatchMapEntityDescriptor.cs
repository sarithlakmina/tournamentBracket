using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Persistence.EntityConfiguration;

public class TournamentMatchMapEntityDescriptor : IEntityTypeConfiguration<TournamentMatchMap>
{
    public void Configure(EntityTypeBuilder<TournamentMatchMap> builder)
    {
        builder.HasKey(ttm => ttm.TournamentMatchMapID);

        builder.HasIndex(ttm => ttm.TournamentMatchMapID);

        builder.HasOne(t => t.Tournament)
             .WithMany(tm => tm.TournamentMatchMaps)
             .HasForeignKey(t => t.TournamentID)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);

        builder.HasOne(m => m.Match)
             .WithMany(tm => tm.TournamentMatchMaps)
             .HasForeignKey(m => m.MatchID)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);
    }
}
