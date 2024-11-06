using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnAim.Admin.Domain.Entities;

namespace OnAim.Admin.Infrasturcture.Persistance.DbConfiguration;

public class AppSettingEntityConfiguration : IEntityTypeConfiguration<AppSetting>
{
    public void Configure(EntityTypeBuilder<AppSetting> builder)
    {
        builder.ToTable("AppSettings")
        .HasKey(a => a.Id);

        builder
            .Property(a => a.Name)
            .IsRequired()
        .HasMaxLength(100);

        builder
            .Property(a => a.Value)
            .IsRequired();
    }
}
