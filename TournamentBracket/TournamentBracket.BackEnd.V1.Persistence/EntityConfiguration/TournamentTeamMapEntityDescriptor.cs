using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Persistence.EntityConfiguration;

public class TournamentTeamMapEntityDescriptor : IEntityTypeConfiguration<TournamentTeamMap>
{
    public void Configure(EntityTypeBuilder<TournamentTeamMap> builder)
    {
        builder.HasKey(ttm => ttm.TournamentTeamMapID);

        builder.HasIndex(ttm => ttm.TournamentTeamMapID);

        builder.HasOne(t => t.Tournament)
             .WithMany(tm => tm.TournamentTeamMaps)
             .HasForeignKey(t => t.TournamentID)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);

        builder.HasOne(t => t.Team)
             .WithMany(tm => tm.TournamentTeamMaps)
             .HasForeignKey(t => t.TeamID)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);
    }
}
