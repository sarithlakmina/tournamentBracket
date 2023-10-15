using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Persistence.EntityConfiguration;

public class MatchCategoryEntityDescriptor : IEntityTypeConfiguration<MatchCategory>
{
    public void Configure(EntityTypeBuilder<MatchCategory> builder)
    {
        builder.HasKey(m => m.MatchCategoryID);

        builder.Property(m => m.Name)
            .IsRequired(true)
            .HasMaxLength(250);

        builder.HasOne(m => m.Tournament)
                .WithMany()
                .HasForeignKey(m => m.TournamentID)
                .IsRequired(true);
    }
}
