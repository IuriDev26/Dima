namespace Dima.Core.Requests.Orders;

public class PayOrderRequest(string orderNumber) : Request
{
    public string OrderNumber { get; set; } = orderNumber;
}