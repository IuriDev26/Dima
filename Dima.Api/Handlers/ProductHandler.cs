using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Dima.Api.Handlers;

public class ProductHandler(AppDbContext context) : IProductHandler
{
    public async Task<PagedResponse<List<Product>?>> GetAllAsync(GetAllProductsRequest request)
    {
        var products = await context.Products
            .AsNoTracking()
            .Where(product => product.IsActive)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return products.IsNullOrEmpty()
            ? new PagedResponse<List<Product>?>(null, 400, "Nenhum produto encontrado")
            : new PagedResponse<List<Product>?>(products, request.PageNumber, request.PageSize, products.Count );
    }

    public async Task<Response<Product?>> GetByIdAsync(GetProductByIdRequest request)
    {
        var product = await context.Products
            .AsNoTracking()
            .Where(product => product.IsActive)
            .FirstOrDefaultAsync(p => p.Id == request.Id);
        
        return product is null
            ? new Response<Product?>(null, 404, "Nenhum produto encontrado")
            : new Response<Product?>(product, 200);
    }
}