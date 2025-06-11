using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.App.Pages.Products;

public partial class Index : ComponentBase
{
    #region Properties

    public bool IsLoading { get; set; }
    private List<Product> Products { get; set; } = [];
    public Order? Order { get; set; }

    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject] 
    public IProductHandler ProductHandler { get; set; } = null!;

    [Inject]
    public IOrderHandler OrderHandler { get; set; } = null!;

    [Inject] 
    public NavigationManager Navigator { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        try
        {
            IsLoading = true;

            var request = new GetAllProductsRequest();
            var response = await ProductHandler.GetAllAsync(request);

            if (response.IsSuccess)
                Products = response.Data!;
            else
                Snackbar.Add(response.Message ?? string.Empty, Severity.Error);
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

    private async Task CreateOrder(long productId)
    {
        try
        {
            IsLoading = true;

            var request = new CreateOrderRequest(productId);
            var response = await OrderHandler.CreateAsync(request);
            
            if (response.IsSuccess)
            {
                Order = response.Data!;
                
                Snackbar.Add("Pedido criado com sucesso!", Severity.Success);
                Navigator.NavigateTo($"premium/checkout/{Order.Number}");
            }
            else
            {
                Snackbar.Add("Erro ao criar seu pedido", Severity.Error);
                Console.WriteLine(response.Message);
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