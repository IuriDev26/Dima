using Dima.Api.Data;
using Dima.Core.Common.Extensions;
using Dima.Core.Enums;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class TransactionHandler(AppDbContext context) : ITransactionHandler
{
    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        try
        {
            request.Amount = request.Type == ETransactionType.Withdraw ? -(request.Amount) : request.Amount;
            
            var transaction = new Transaction()
            {
                CategoryId = request.CategoryId,
                Amount = request.Amount,
                PaidOrReceivedAt = request.PaidOrReceivedAt,
                Title = request.Title,
                Type = request.Type,
                UserId = request.UserId
            };

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction);
        }
        catch
        {
            return new Response<Transaction?>(null, message: "An error occured", code:500);
        }
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        try
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(transaction => transaction.Id == request.Id);
            
            if (transaction == null)
                return new Response<Transaction?>(null, message: "Transaction not found", code:500);

            transaction.CategoryId = request.CategoryId;
            transaction.Amount = request.Amount;
            transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;
            transaction.Title = request.Title;
            transaction.Type = request.Type;
            transaction.UserId = request.UserId;
            
            context.Transactions.Update(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction);
        }
        catch
        {
            return new Response<Transaction?>(null, message: "An error occured", code:500);
        }
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        try
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(transaction => transaction.Id == request.Id
            && transaction.UserId == request.UserId);
            
            if (transaction == null)
                return new Response<Transaction?>(null, message: "Transaction not found", code:500);
            
            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction);
        }
        catch
        {
            return new Response<Transaction?>(null, message: "An error occured", code:500);
        }
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetByIdRequest request)
    {
        try
        {
            var transaction = await context.Transactions.AsNoTracking()
                .FirstOrDefaultAsync(transaction => transaction.Id == request.Id 
                                                    && transaction.UserId == request.UserId);
            
            return transaction == null 
                ? new Response<Transaction?>(null, message: "Transaction not found", code:500) 
                : new Response<Transaction?>(transaction);
        }
        catch
        {
            return new Response<Transaction?>(null, message: "An error occured", code:500);
        }
    }

    public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetByPeriodRequest request)
    {
        try
        {
            var initialInterval = request.InitialInterval ?? DateTime.Now.FirstDay();
            var finalInterval = request.FinalInterval ?? DateTime.Now.LastDay();
            
            var transactions = await context.Transactions
                .AsNoTracking()
                .Where(transaction => transaction.PaidOrReceivedAt >= initialInterval &&
                                      transaction.PaidOrReceivedAt <= finalInterval &&
                                      transaction.UserId == request.UserId)
                .ToListAsync();

            return transactions.Count == 0 
                ? new PagedResponse<List<Transaction>?>(null, message: "Não foram encontradas transações para o filtro especificado") 
                : new PagedResponse<List<Transaction>?>(transactions!, request.PageNumber, request.PageSize, transactions.Count);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new PagedResponse<List<Transaction>?>(null, 500, "Erro ao buscar transações");
        }
    }
}