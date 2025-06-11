using Dima.Core.Handlers;
using Dima.Core.Requests.Stripe;
using Dima.Core.Responses;
using Dima.Core.Responses.Stripe;
using Stripe;
using Stripe.Checkout;

namespace Dima.Api.Handlers;

public class StripeHandler : IStripeHandler
{
    public async Task<Response<string?>> CreateSessionAsync(CreateSessionRequest request)
    {
        var options = new SessionCreateOptions()
        {
            CustomerEmail = request.UserId,
            PaymentIntentData = new SessionPaymentIntentDataOptions
            {
                Metadata = new Dictionary<string, string>()
                {
                    { "order", request.OrderNumber }
                }
            },
            PaymentMethodTypes = 
            [
                "card"
            ],
            LineItems = 
            [
                new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        Currency = "BRL",
                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = request.ProductTitle,
                            Description = request.ProductDescription
                        },
                        UnitAmount = request.OrderTotal
                    },
                    Quantity = 1
                }    
            ],
            Mode = "payment",
            SuccessUrl = $"{Configuration.FrontEndUrl}/premium/payment-success?orderNumber={request.OrderNumber}",
            CancelUrl = $"{Configuration.FrontEndUrl}/premium/payment-fail?orderNumber={request.OrderNumber}"
        };
        
        var service = new SessionService();
        var session = await service.CreateAsync(options);
        
        return new Response<string?>(session.Id, 200, "Sessão Stripe criada com sucesso!");
    }

    public async Task<Response<List<StripeTransactionResponse>>> GetTransactionsByOrderNumberAsync(GetTransactionsByOrderNumberRequest request)
    {
        var options = new ChargeSearchOptions()
        {
            Query = $"metadata['order']:'{request.Number}'"
        };
        
        var service = new ChargeService();
        var response = await service.SearchAsync(options);

        if (response is null)
            return new Response<List<StripeTransactionResponse>>(null, 500, "Erro ao buscar transações");
        
        if (response.Data.Count == 0)
            return new Response<List<StripeTransactionResponse>>(null, 500, "Erro ao buscar transações");
        
        var transactions = response.Data.Select(transaction => new StripeTransactionResponse()
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                AmountCaptured = transaction.AmountCaptured,
                Status = transaction.Status,
                Paid = transaction.Paid,
                Email = transaction.BillingDetails.Email,
                Refund = transaction.Refunded
            })
            .ToList();

        return new Response<List<StripeTransactionResponse>>(transactions, 200);
    }
}