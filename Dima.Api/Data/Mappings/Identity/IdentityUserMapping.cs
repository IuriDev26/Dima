using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings.Identity;

public class IdentityUserMapping : IEntityTypeConfiguration<IdentityUser<long>>
{
    public void Configure(EntityTypeBuilder<IdentityUser<long>> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.NormalizedEmail).IsUnique();
        builder.HasIndex(x => x.NormalizedUserName).IsUnique();
        
        builder.Property(x => x.Email)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.Property(x => x.UserName)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.Property(x => x.NormalizedEmail)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.Property(x => x.NormalizedUserName)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.Property(x => x.ConcurrencyStamp)
            .IsConcurrencyToken();

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(13);
        
        builder.HasMany<IdentityUserRole<long>>().WithOne().HasForeignKey(x => x.UserId).IsRequired();
        builder.HasMany<IdentityUserClaim<long>>().WithOne().HasForeignKey(x => x.UserId).IsRequired();
        builder.HasMany<IdentityUserLogin<long>>().WithOne().HasForeignKey(x => x.UserId).IsRequired();
        builder.HasMany<IdentityUserToken<long>>().WithOne().HasForeignKey(x => x.UserId).IsRequired();
            
    }
}