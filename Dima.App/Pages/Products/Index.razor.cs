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
    public List<Product> Products { get; set; } = [];

    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject] 
    public IProductHandler Handler { get; set; } = null!;

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
            var response = await Handler.GetAllAsync(request);

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


}