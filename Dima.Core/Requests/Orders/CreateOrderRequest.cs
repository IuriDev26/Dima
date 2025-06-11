namespace Dima.Core.Requests.Orders;

public class CreateOrderRequest(long productId, long? voucherId = null) : Request
{
    public long ProductId { get; set; } = productId;
    public long? VoucherId { get; set; } = voucherId;
}