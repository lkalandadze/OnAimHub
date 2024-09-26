using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnAim.Admin.Domain.Entities;

namespace OnAim.Admin.Infrasturcture.Persistance.Data.DbConfiguration;

public class EndpointGroupEndpointEntityConfiguration : IEntityTypeConfiguration<EndpointGroupEndpoint>
{
    public void Configure(EntityTypeBuilder<EndpointGroupEndpoint> builder)
    {
        builder.HasKey(ege => new { ege.EndpointGroupId, ege.EndpointId });

        builder
            .HasOne(ege => ege.EndpointGroup)
            .WithMany(eg => eg.EndpointGroupEndpoints)
            .HasForeignKey(ege => ege.EndpointGroupId);

        builder
            .HasOne(ege => ege.Endpoint)
            .WithMany(e => e.EndpointGroupEndpoints)
            .HasForeignKey(ege => ege.EndpointId);
    }
}
