namespace Dima.Core.Requests.Orders;

public class CancelOrderRequest(string orderNumber) : Request
{
    public string OrderNumber { get; set; } = orderNumber;
}