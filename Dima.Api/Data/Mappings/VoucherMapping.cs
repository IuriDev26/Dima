using Dima.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings;

public class VoucherMappingv : IEntityTypeConfiguration<Voucher>
{
    public void Configure(EntityTypeBuilder<Voucher> builder)
    {
        builder.ToTable("Voucher");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Number)
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(15);
        
        builder.Property(x => x.Title)
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(30);
        
        builder.Property(x => x.Description)
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(255);
        
        builder.Property(x => x.StartDate)
            .IsRequired()
            .HasColumnType("DATETIME2");
        
        builder.Property(x => x.EndDate)
            .IsRequired()
            .HasColumnType("DATETIME2");
        
        builder.Property(x => x.Amount)
            .IsRequired()
            .HasColumnType("MONEY");
    }
}