using Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hub.Infrastructure.TypeConfigurations;

public class PromotionViewTypeConfiguration : IEntityTypeConfiguration<PromotionView>
{
    public void Configure(EntityTypeBuilder<PromotionView> builder)
    {
        builder.Property(s => s.PromotionViewTemplateId)
               .IsRequired(false);
    }
}