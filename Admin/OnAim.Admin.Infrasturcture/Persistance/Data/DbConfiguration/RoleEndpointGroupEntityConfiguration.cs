using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnAim.Admin.Domain.Entities;
namespace OnAim.Admin.Infrasturcture.Persistance.Data.DbConfiguration;

public class RoleEndpointGroupEntityConfiguration : IEntityTypeConfiguration<RoleEndpointGroup>
{
    public void Configure(EntityTypeBuilder<RoleEndpointGroup> builder)
    {
        builder.HasKey(reg => new { reg.RoleId, reg.EndpointGroupId });

        builder
            .HasOne(reg => reg.Role)
            .WithMany(r => r.RoleEndpointGroups)
            .HasForeignKey(reg => reg.RoleId);

        builder
            .HasOne(reg => reg.EndpointGroup)
            .WithMany(eg => eg.RoleEndpointGroups)
            .HasForeignKey(reg => reg.EndpointGroupId);
    }
}
