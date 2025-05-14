using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings.Identity;

public class IdentityUserLoginMapping : IEntityTypeConfiguration<IdentityUserLogin<long>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<long>> builder)
    {
        builder.ToTable("UserLogins");
        builder.HasKey(p => new { p.UserId, p.ProviderKey, p.LoginProvider });
        
        builder.Property(p => p.ProviderKey).IsRequired();
        builder.Property(p => p.LoginProvider).IsRequired();
        builder.Property(p => p.ProviderDisplayName).HasMaxLength(50).IsRequired();

    }
}