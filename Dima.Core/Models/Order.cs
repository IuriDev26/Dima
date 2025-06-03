using Dima.Core.Enums;

namespace Dima.Core.Models;

public class Order
{
    public long Id { get; set; }
    public string Number { get; set; } = Guid.NewGuid().ToString("N")[..8];
    public string UserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public long ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public long? VoucherId { get; set; }
    public Voucher? Voucher { get; set; }
    public decimal Total => Product.Amount - Voucher?.Amount ?? 0;
    public EPaymentGateway PaymentGateway { get; set; } = EPaymentGateway.Stripe;
    public string? ExternalReference { get; set; }
    public EOrderStatus Status { get; set; } = EOrderStatus.Opened;

}