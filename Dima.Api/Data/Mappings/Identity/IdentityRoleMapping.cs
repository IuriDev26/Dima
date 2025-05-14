using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings.Identity;

public class IdentityRoleMapping : IEntityTypeConfiguration<IdentityRole<long>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<long>> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.ConcurrencyStamp).IsConcurrencyToken();
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.NormalizedName).HasMaxLength(200).IsRequired();
    }
}