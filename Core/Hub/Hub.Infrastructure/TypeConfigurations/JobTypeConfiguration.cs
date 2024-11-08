using Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hub.Infrastructure.TypeConfigurations;

public class JobTypeConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.Property(e => e.CurrencyId).IsRequired(false);
    }
}