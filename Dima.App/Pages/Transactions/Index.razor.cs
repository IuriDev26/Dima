using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.App.Pages.Transactions;

public partial class Index : ComponentBase
{
    #region Properties

    private List<Transaction> Transactions { get; set; } = [];

    private int PageNumber { get; set; } = 1;

    private int PageSize { get; set; } = 10;
    
    private int SearchMonth { get; set; } = DateTime.Now.Month;
    
    private int SearchYear { get; set; } = DateTime.Now.Year;
    
    private bool IsLoading { get; set; }
    
    public string SearchString { get; set; } = string.Empty;
    
    private List<DateTime> Years { get; set; } = [];
    
    #endregion

    #region Services

    [Inject]
    private ITransactionHandler TransactionHandler { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;
    
    [Inject]
    private IDialogService DialogService { get; set; } = null!;
    
    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync() => await GetTransactionsAsync();
    

    #endregion

    #region Methods

    private Func<Transaction, bool> Filter
        => transaction => string.IsNullOrEmpty(SearchString) ||
                          transaction.Title.Contains(SearchString, StringComparison.InvariantCultureIgnoreCase);

    private async Task DeleteAsync(long id)
    {
        try
        {
            IsLoading = true;

            var request = new DeleteTransactionRequest { Id = id };
            var response = await TransactionHandler.DeleteAsync(request);

            if (response.IsSuccess)
            {
                Snackbar.Add(response.Message, Severity.Success);
                Transactions.RemoveAll(transaction => transaction.Id == id);
                StateHasChanged();
            }
            else
            {
                Snackbar.Add(response.Message, Severity.Error);
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

    private async Task OnClickDeleteButtonAsync(long id, string title)
    {
        var userChoice = await DialogService.ShowMessageBox("Atenção",$"Deseja realmente excluir a transação    {title}",
            yesText: "Excluir",
            cancelText: "Cancelar");

        if (userChoice != null && userChoice.Value)
            await DeleteAsync(id);
    }

    private async Task GetTransactionsAsync()
    {
        try
        {
            IsLoading = true;

            var baseDate = new DateTime(SearchYear, SearchMonth, 1);
            
            var request = new GetByPeriodRequest
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
                InitialInterval = baseDate,
                FinalInterval = new DateTime(SearchYear, SearchMonth, baseDate.LastDay().Day)
            };
            
            var response = await TransactionHandler.GetByPeriodAsync(request);

            if (response.IsSuccess)
                Transactions = response.Data ?? [];
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
    
    public async Task OnClickSearchButtonAsync() => await GetTransactionsAsync();

    #endregion


}