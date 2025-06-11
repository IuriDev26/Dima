using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.App.Pages.Premium;

public partial class PaymentSuccess : ComponentBase
{
    #region Properties

    [SupplyParameterFromQuery]
    public string OrderNumber { get; set; } = string.Empty;

    public Order Order { get; set; } = null!;

    public bool IsLoading { get; set; }

    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    public IOrderHandler OrderHandler { get; set; } = null!;

    [Inject]
    public IStripeHandler StripeHandler { get; set; } = null!;

    [Inject] public NavigationManager Navigator { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        try
        {
            IsLoading = true;

            var request = new PayOrderRequest(OrderNumber);
            var response = await OrderHandler.PayAsync(request);

            if (!response.IsSuccess)
            {
                Snackbar.Add(response.Message, Severity.Error);
                return;
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
    
}