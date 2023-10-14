using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Persistence.EntityConfiguration;

public class MatchEntityDescriptor : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.HasKey(m => m.MatchID);

        builder.HasOne(m => m.Tournament)
                .WithMany()
                .HasForeignKey(m => m.TournamentID)
                .IsRequired(true);

        builder.Property(m => m.HomeTeamID)
            .IsRequired(true);

        builder.Property(m => m.AwayTeamID)
            .IsRequired(true);
        builder.Property(m => m.WinningTeamID)
            .IsRequired(false);
    }
}
