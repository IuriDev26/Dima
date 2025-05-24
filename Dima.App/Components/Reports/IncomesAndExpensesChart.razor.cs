using System.Runtime.InteropServices;
using Dima.Core.Handlers;
using Dima.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.App.Components.Reports;

public partial class IncomesAndExpensesChart : ComponentBase
{
    #region Properties

    private List<ChartSeries> Data { get; set; } = [];
    
    private List<double> Incomes { get; set; } = [];
    private List<double> Expenses { get; set; } = [];
    
    private List<string> Months { get; set; } = [];
    
    private bool IsLoading { get; set; }

    private AxisChartOptions Options { get; set; } = new();

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
            await Task.Delay(2500);
            var request = new GetIncomesAndExpensesRequest();
            var response = await Handler.GetIncomesAndExpensesReportAsync(request);

            if (!response.IsSuccess || response.Data is null)
            {
                Snackbar.Add(response.Message, Severity.Error);
                return;
            }

            foreach (var data in response.Data)
            {

                Months.Add(GetMonthName(data.Month));
                Incomes.Add((double)data.Incomes);
                Expenses.Add(-(double)data.Expenses);

            }

            Data.Add(new ChartSeries()
            {
                Name = "Incomes",
                Data = Incomes.ToArray(),
                ShowDataMarkers = true
            });

            Data.Add(new ChartSeries()
            {
                Name = "Expenses",
                Data = Expenses.ToArray(),
                ShowDataMarkers = true
            });

            Options.MatchBoundsToSize = true;

        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
            Console.WriteLine(ex.Message + ex.StackTrace);
        }
        finally
        {
            IsLoading = false;
        }
        
    }

    #endregion

    #region Methods

    private string GetMonthName(int month) => new DateTime(DateTime.Now.Year, month, 1).ToString("MMMM");

    #endregion
}