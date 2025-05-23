using Dima.Core.Enums;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.App.Pages.Transactions;

public partial class Update : ComponentBase
{
    #region Properties

    private UpdateTransactionRequest Transaction  { get; set; } = new();
    
    private List<Category> Categories { get; set; } = [];
    
    private bool IsLoading { get; set; }
    
    [Parameter]
    public long Id { get; set; }

    #endregion

    #region Services

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private ITransactionHandler TransactionHandler { get; set; } = null!;
    
    [Inject]
    private ICategoryHandler CategoryHandler { get; set; } = null!;
    
    [Inject]
    private NavigationManager Navigator { get; set; } = null!;
    
    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        try
        {
            IsLoading = true;
            
            await GetCategoriesAsync();

            var response = await TransactionHandler.GetByIdAsync(new GetByIdRequest { Id = Id });

            if (response.IsSuccess)
            {
                Transaction.Id = response.Data!.Id;
                Transaction.Title = response.Data!.Title;
                Transaction.CategoryId = response.Data!.CategoryId;
                Transaction.Type = response.Data!.Type;
                Transaction.Amount = response.Data!.Amount;
                Transaction.PaidOrReceivedAt = response.Data!.PaidOrReceivedAt;
            }
            else
            {
                Snackbar.Add(response.Message, Severity.Error);
                Navigator.NavigateTo("/transactions");
            }

        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    #endregion

    #region Methods

    private async Task UpdateTransaction()
    {
        try
        {
            IsLoading = true;

            var response = await TransactionHandler.UpdateAsync(Transaction);

            if (response.IsSuccess)
            {
                Snackbar.Add(response.Message, Severity.Success);
                Navigator.NavigateTo("/transactions");
            }
            else
            {
                Snackbar.Add(response.Message, Severity.Error);
            }

        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task GetCategoriesAsync()
    {
        var response = await CategoryHandler.GetAllAsync(new GetAllCategoriesRequest
        {
            PageNumber = 1,
            PageSize = 25
        });

        Categories = response.Data ?? [];
    }

    #endregion
    
}