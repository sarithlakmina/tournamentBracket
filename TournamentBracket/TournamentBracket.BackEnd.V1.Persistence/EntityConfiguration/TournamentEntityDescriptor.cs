using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Persistence.EntityConfiguration;

public sealed class TournamentEntityDescriptor : IEntityTypeConfiguration<Tournament>
{
    public void Configure(EntityTypeBuilder<Tournament> builder)
    {
        builder.HasKey(t => t.TournamentID);

        builder.Property(t => t.Name)
            .IsRequired(true)
            .HasMaxLength(250);
    }
}
