using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnAim.Admin.Domain.Entities;

namespace OnAim.Admin.Infrasturcture.Persistance.DbConfiguration;

public class AccessTokenEntityConfiguration : IEntityTypeConfiguration<AccessToken>
{
    public void Configure(EntityTypeBuilder<AccessToken> builder)
    {
        builder.HasIndex(at => at.Token)
               .IsUnique();
    }
}
