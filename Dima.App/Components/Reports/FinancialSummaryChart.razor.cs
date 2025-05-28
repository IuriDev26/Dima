using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.App.Components.Reports;

public partial class FinancialSummaryChart : ComponentBase
{

    #region Properties

    private bool IsLoading { get; set; }

    private FinancialSummary FinancialSummary { get; set; } = null!;
    
    private bool IsValueVisible { get; set; } = true;

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

            var request =  new GetFinancialSummaryRequest();
            var response = await Handler.GetFinancialSummaryReportAsync(request);

            if (!response.IsSuccess)
            {
                Snackbar.Add(response.Message, Severity.Error);
                return;
            }

            FinancialSummary = response.Data!;
            StateHasChanged();


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

    public void OnChangeVisibility()
    {
        IsValueVisible = !IsValueVisible;
        StateHasChanged();
    }

    public string GetVisibilityIcon()
    {
        return IsValueVisible ? Icons.Material.Filled.VisibilityOff : Icons.Material.Filled.Visibility;
    }

    #endregion
}