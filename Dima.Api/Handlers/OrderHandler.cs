using Dima.Api.Data;
using Dima.Core.Enums;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Requests.Stripe;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Dima.Api.Handlers;

public class OrderHandler(AppDbContext context, IStripeHandler stripeHandler) : IOrderHandler
{
    public async Task<Response<Order?>> CancelAsync(CancelOrderRequest request)
    {
        var order = await context.Orders
            .Include(x => x.Product)
            .Include(x => x.Voucher)
            .FirstOrDefaultAsync(o => o.Number == request.OrderNumber && 
                                      o.UserId == request.UserId);

        if (order is null)
            return new Response<Order?>(null, 404, "Pedido não encontrado");

        switch (order.Status)
        {
            case EOrderStatus.Canceled:
                return new Response<Order?>(null, 400, "Pedido já foi cancelado anteriormente'");
            case EOrderStatus.Paid:
                return new Response<Order?>(null, 400, "Esse pedido já foi pago e não pode ser cancelado");
            case EOrderStatus.Opened:
                break;
            case EOrderStatus.Refunded:
                return new Response<Order?>(null, 400, "Esse pedido já foi estornado");
            default:
                return new Response<Order?>(null, 400, "Esse pedido não pode ser cancelado");
        }
        
        order.Status = EOrderStatus.Canceled;

        context.Update(order);
        await context.SaveChangesAsync();
        
        return new Response<Order?>(order, 200, "Pedido Cancelado com Sucesso");
    }

    public async Task<Response<Order?>> CreateAsync(CreateOrderRequest request)
    {
        Product? product;
        try
        {
            product = context.Products.FirstOrDefault(p => p.Id == request.ProductId && p.IsActive);
            
            if (product is null)
                return new Response<Order?>(null, 404, "Produto não encontrado");

            context.Attach(product);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Response<Order?>(null, 500);
        }

        Voucher? voucher = null;
        try
        {
            if (request.VoucherId is not null)
            {
                voucher = context.Vouchers.FirstOrDefault(v => v.Id == request.ProductId && v.IsActive);

                if (voucher is null)
                    return new Response<Order?>(null, 404, "Voucher não encontrado");

                context.Attach(voucher);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Response<Order?>(null, 500);
        }

        Order? order;
        try
        {
            order = new Order
            {
                UserId = request.UserId,
                ProductId = request.ProductId,
                Product = product,
                VoucherId = request.VoucherId,
                Voucher = voucher
            };

            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Response<Order?>(null, 500);
        }
        
        return new Response<Order?>(order, 200, "Pedido criado com sucesso");
    }

    public async Task<PagedResponse<List<Order>?>> GetAllAsync(GetAllOrdersRequest request)
    {
        var orders = await context.Orders
            .AsNoTracking()
            .Where(order => order.UserId == request.UserId)
            .Include(x => x.Product)
            .Include(x => x.Voucher)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
        
        return orders.IsNullOrEmpty()
            ? new PagedResponse<List<Order>?>(null, 404, "Nenhum registro encontrado")
            : new PagedResponse<List<Order>?>(orders, request.PageNumber, request.PageSize, orders.Count);
    }

    public async Task<Response<Order?>> GetByNumberAsync(GetOrderByNumberRequest request)
    {
        var order = await context.Orders
            .AsNoTracking()
            .Where(order => order.UserId == request.UserId)
            .Include(x => x.Product)
            .Include(x => x.Voucher)
            .FirstOrDefaultAsync(order => order.Number == request.Number);

        return order is null
            ? new Response<Order?>(null, 404, "Pedido não encontrado")
            : new Response<Order?>(order, 200);
    }

    public async Task<Response<Order?>> PayAsync(PayOrderRequest request)
    {
        Order? order;

        try
        {
            order = await context.Orders
                .Include(x => x.Product)
                .Include(x => x.Voucher)
                .FirstOrDefaultAsync(o => o.Number == request.OrderNumber && o.UserId == request.UserId);

            if (order is null)
                return new Response<Order?>(null, 404, "Pedido não encontrado");

            switch (order.Status)
            {
                case EOrderStatus.Refunded:
                    return new Response<Order?>(null, 400, "Esse pedido já foi reembolsado");
                
                case EOrderStatus.Canceled:
                    return new Response<Order?>(null, 400, "Esse pedido foi cancelado");
                
                case EOrderStatus.Paid:
                    return new Response<Order?>(null, 400, "Esse pedido já foi pago");
                
                case EOrderStatus.Opened:
                    break;
                
                default:
                    return new Response<Order?>(null, 400, "Não foi possível realizar o pagamento deste pedido");
            }

            string externalReference;
            try
            {
                var getStripeTransactionsRequest = new GetTransactionsByOrderNumberRequest(request.OrderNumber);
                var response = await stripeHandler.GetTransactionsByOrderNumberAsync(getStripeTransactionsRequest);

                if (!response.IsSuccess)
                    return new Response<Order?>(null, 500, "Não foi possível obter as transações no stripe");

                if (response.Data!.Any(transaction => transaction.Refund))
                    return new Response<Order?>(null, 400, "Esse pedido foi estornado");
                
                if (!response.Data!.Any(transaction => transaction.Paid))
                    return new Response<Order?>(null, 400, "Não foram encontrados pagamentos para esse pedido");

                externalReference = response.Data![0].Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Response<Order?>(null, 500, "Falha ao buscar pagamentos do pedido");
            }
            
            order.Status = EOrderStatus.Paid;
            order.UpdatedAt = DateTime.Now;
            order.ExternalReference = externalReference;

            context.Update(order);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Response<Order?>(null, 500);
        }

        return new Response<Order?>(order, 200, "Pedido pago com sucesso");
    }

    public async Task<Response<Order?>> RefundAsync(RefundOrderRequest request)
    {
        Order? order;
        try
        {
            order = await context.Orders
                .Include(x => x.Product)
                .Include(x => x.Voucher)
                .FirstOrDefaultAsync(o => o.Id == request.Id && o.UserId == request.UserId);

            if (order is null)
                return new Response<Order?>(null, 404, "Pedido não encontrado");

            switch (order.Status)
            {
                case EOrderStatus.Refunded:
                    return new Response<Order?>(null, 400, "Esse pedido já foi reembolsado");
                
                case EOrderStatus.Canceled:
                    return new Response<Order?>(null, 400, "Esse pedido foi cancelado");
                
                case EOrderStatus.Paid:
                    break;
                
                case EOrderStatus.Opened:
                    return new Response<Order?>(null, 400, "Esse pedido ainda não foi pago");
                
                default:
                    return new Response<Order?>(null, 400, "Não foi possível realizar o pagamento deste pedido");
            }
            
            order.Status = EOrderStatus.Refunded;
            order.UpdatedAt = DateTime.Now;
            
            context.Update(order);
            await context.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Response<Order?>(null, 500);
        }

        return new Response<Order?>(order, 200, "Pedido estornado com sucesso");
    }
}