using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings.Identity;

public class IdentityRoleClaimMapping : IEntityTypeConfiguration<IdentityRoleClaim<long>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<long>> builder)
    {
        builder.ToTable("RoleClaims");
        builder.HasKey(c => new { c.Id, c.RoleId });
        
        builder.Property(c => c.ClaimType).IsRequired();
        builder.Property(c => c.ClaimValue).IsRequired();
    }
}