using Dima.Core.Enums;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.App.Pages.Transactions;

public partial class Create : ComponentBase
{

    #region Properties

    private CreateTransactionRequest Transaction  { get; set; } = new();
    
    private List<Category> Categories { get; set; } = [];
    
    private bool IsLoading { get; set; }

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

            var response = await CategoryHandler.GetAllAsync(new GetAllCategoriesRequest
            {
                PageNumber = 1,
                PageSize = 25
            });

            Categories = response.Data ?? [];
            
            Transaction.CategoryId = Categories.FirstOrDefault()!.Id;
            Transaction.Type = ETransactionType.Withdraw;
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

    private async Task CreateTransaction()
    {
        try
        {
            IsLoading = true;

            var response = await TransactionHandler.CreateAsync(Transaction);

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

    #endregion
    
}