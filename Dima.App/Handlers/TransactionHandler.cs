using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.App.Handlers;

public class TransactionHandler : ITransactionHandler
{
    public Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Transaction?>> GetByIdAsync(GetByIdRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResponse<List<Transaction?>>> GetByPeriodAsync(GetByPeriodRequest request)
    {
        throw new NotImplementedException();
    }
}