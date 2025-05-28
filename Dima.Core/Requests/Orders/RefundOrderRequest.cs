using System.Security.AccessControl;

namespace Dima.Core.Requests.Orders;

public class RefundOrderRequest : Request
{
    public long Id { get; set; }
}