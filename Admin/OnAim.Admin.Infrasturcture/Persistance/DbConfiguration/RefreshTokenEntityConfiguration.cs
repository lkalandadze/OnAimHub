using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnAim.Admin.Domain.Entities;

namespace OnAim.Admin.Infrasturcture.Persistance.DbConfiguration;

public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasIndex(rt => rt.Token)
            .IsUnique();
    }
}
