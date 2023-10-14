using Microsoft.EntityFrameworkCore;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Persistence.EntityConfiguration;

public class MatchMatchCategoryMapEntityDescriptor : IEntityTypeConfiguration<MatchMatchCategoryMap>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<MatchMatchCategoryMap> builder)
    {
        builder.HasKey(m => m.MatchMatchCategoryMapID);

        builder.HasIndex(m => m.MatchMatchCategoryMapID);

        builder.HasOne(m => m.Match)
             .WithMany(tm => tm.MatchMatchCategoryMaps)
             .HasForeignKey(t => t.MatchID)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);

        builder.HasOne(m => m.Tournament)
             .WithMany(tm => tm.MatchMatchCategoryMaps)
             .HasForeignKey(t => t.TournamentID)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);

        builder.HasOne(t => t.MatchCategory)
             .WithMany(tm => tm.MatchMatchCategoryMaps)
             .HasForeignKey(t => t.MatchCategoryID)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);
    }
}
