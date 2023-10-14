using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Persistence.EntityConfiguration;

public sealed class TeamEntityDescriptor : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasKey(t => t.TeamID);

        builder.Property(t => t.Name)
            .IsRequired(true)
            .HasMaxLength(250);
    }
}
