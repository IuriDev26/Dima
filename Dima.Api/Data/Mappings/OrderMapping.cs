using Dima.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings;

public class OrderMapping : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Number)
            .IsRequired()
            .HasColumnType("CHAR")
            .HasMaxLength(8);
        
        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(200);
        
        builder.Property(x=> x.CreatedAt)
            .IsRequired()
            .HasColumnType("DATETIME2");
        
        builder.Property(x=> x.UpdatedAt)
            .IsRequired()
            .HasColumnType("DATETIME2");

        builder.Property(x => x.PaymentGateway)
            .IsRequired();
        
        builder.Property(x => x.ExternalReference)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(200);

        builder.Property(x => x.Status)
            .IsRequired();

        builder.HasOne(x => x.Product).WithMany();
        builder.HasOne(x => x.Voucher).WithMany();
    }
}