namespace Dima.Core.Requests.Stripe;

public class GetTransactionsByOrderNumberRequest(string number) : Request
{
    public string Number { get; set; } = number;
}