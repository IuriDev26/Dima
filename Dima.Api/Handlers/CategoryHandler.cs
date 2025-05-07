using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class CategoryHandler : ICategoryHandler
{
    private readonly AppDbContext _context;
    
    public CategoryHandler(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        try
        {
            var category = new Category()
            {
                Title = request.Title,
                Description = request.Description,
                UserId = request.UserId
            };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            
            return new Response<Category?>(category, 201, "Categoria criada com sucesso");
        }
        catch (Exception ex)
        {
            return new Response<Category?>(data:null, 500, "Erro ao criar categoria");
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            var category = _context.Categories.FirstOrDefault(
                category => category.Id == request.Id && category.UserId == request.UserId
            );

            if (category is null)
                return new Response<Category?>(data:null, 400, "Categoria não encontrada");
            
            category.Title = request.Title;
            category.Description = request.Description;
            category.UserId = request.UserId;
            
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            
            return new Response<Category?>(category, 200, "Categoria atualizada com sucesso");
        }
        catch (Exception ex)
        {
            return new Response<Category?>(data:null, 500, "Erro ao atualizar categoria");
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            var category = _context.Categories.FirstOrDefault(
                category => category.Id == request.Id && category.UserId == request.UserId
            );

            if (category is null)
                return new Response<Category?>(data:null, 400, "Categoria não encontrada");
            
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            
            return new Response<Category?>(category, 200, "Categoria deletada com sucesso");
        }
        catch (Exception ex)
        {
            return new Response<Category?>(data:null, 500, "Erro ao deletar categoria");
        }
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            var category = _context.Categories.AsNoTracking().FirstOrDefault(
                category => category.Id == request.Id && category.UserId == request.UserId
            );

            if (category is null)
                return new Response<Category?>(data:null, 400, "Categoria não encontrada");
            
            return new Response<Category?>(category, 200);
        }
        catch (Exception ex)
        {
            return new Response<Category?>(data:null, 500, "Erro ao deletar categoria");
        }
    }

    public async Task<PagedResponse<List<Category>>> GetAllAsync(GetAllCategoriesRequest request)
    {
        try
        {
            var query = _context.Categories.AsNoTracking().Where(category => category.UserId == request.UserId);
            
            var categories = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();
            var totalCount = await query.CountAsync(); 
            
            return new PagedResponse<List<Category>>(categories, request.PageNumber, request.PageSize, totalCount);
        }
        catch (Exception ex)
        {
            return new PagedResponse<List<Category>>(data:null, 500, "Erro ao retornar categorias");
        }
    }
}