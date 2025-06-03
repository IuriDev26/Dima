using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class VoucherHandler(AppDbContext context) : IVoucherHandler
{
    public async Task<Response<Voucher?>> GetByNumberAsync(GetVoucherByNumberRequest request)
    {
        var voucher = await context.Vouchers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Number == request.Number &&
                                             x.IsActive);

        return voucher is null 
            ? new Response<Voucher?>(null, 404, "Voucher not found") 
            : new Response<Voucher?>(voucher, 200);
    }
}