using Dima.Core.Handlers;
using Dima.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.App.Components.Reports;

public partial class IncomesByCategoryChart : ComponentBase
{
    #region Properties

    private List<double> Incomes { get; set; } = [];
    
    private List<string> Categories { get; set; } = [];
    
    private bool IsLoading { get; set; }
    #endregion
    
    #region Services
    
    [Inject]
    private IReportHandler Handler { get; set; } = null!;
    
    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;
    
    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        try
        {
            IsLoading = true;
            await Task.Delay(4500);
            var request = new GetIncomesByCategoryRequest();
            var response = await Handler.GetIncomesByCategoryReportAsync(request);

            if (!response.IsSuccess || response.Data is null)
            {
                Snackbar.Add(response.Message, Severity.Error);
                return;
            }

            foreach (var data in response.Data)
            {
                Categories.Add(data.Category);
                Incomes.Add((double)data.Incomes);
            }
        
            StateHasChanged();
        }
        finally
        {
            IsLoading = false;
        }
        
    }

    #endregion
}