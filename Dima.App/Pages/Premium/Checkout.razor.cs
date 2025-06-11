using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Requests.Stripe;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace Dima.App.Pages.Premium;

public partial class Checkout : ComponentBase
{
    #region Properties

    [Parameter] 
    public string OrderNumber { get; set; } = string.Empty;

    [SupplyParameterFromQuery]
    public string? VoucherNumber { get; set; }

    private Order Order { get; set; } = new();

    private bool IsLoading { get; set; }

    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    
    [Inject]
    public NavigationManager Navigator { get; set; } = null!;

    [Inject]
    public IProductHandler ProductHandler { get; set; } = null!;

    [Inject]
    public IOrderHandler OrderHandler { get; set; } = null!;

    [Inject]
    public IVoucherHandler VoucherHandler { get; set; } = null!;

    [Inject]
    public IStripeHandler StripeHandler { get; set; } = null!;

    [Inject] 
    public IJSRuntime JsRuntime { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        try
        {
            IsLoading = true;

            var request = new GetOrderByNumberRequest(OrderNumber);
            var response = await OrderHandler.GetByNumberAsync(request);

            if (response.IsSuccess)
                Order = response.Data!;
            else
            {
                Snackbar.Add(response.Message, Severity.Error);
                Navigator.NavigateTo("/premium");
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

    #region Methods
    
    private async Task ApplyVoucher()
    {
        IsLoading = true;
        StateHasChanged();
        await Task.Delay(5000);
        IsLoading = false;
        StateHasChanged();

    }

    private async Task PayOrderAsync()
    {
        IsLoading = true;
        
        var request = new CreateSessionRequest()
        {
            OrderNumber = OrderNumber,
            OrderTotal = (long)Math.Round(Order.Total * 100, 2),
            ProductDescription = Order.Product.Description,
            ProductTitle = Order.Product.Title
        };

        try
        {
            var response = await StripeHandler.CreateSessionAsync(request);
            
            if (!response.IsSuccess)
                Snackbar.Add(response.Message, Severity.Error);
            else
            {
                await JsRuntime.InvokeVoidAsync("checkout", Configuration.StripePublicKey, response.Data);
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