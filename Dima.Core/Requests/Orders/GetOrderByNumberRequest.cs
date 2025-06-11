namespace Dima.Core.Requests.Orders;

public class GetOrderByNumberRequest(string number) : Request
{
    public string Number { get; init; } = number;
}