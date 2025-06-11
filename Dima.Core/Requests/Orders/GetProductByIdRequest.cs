namespace Dima.Core.Requests.Orders;

public class GetProductByIdRequest(long id) : Request
{
    public long Id { get; init; } = id;
}