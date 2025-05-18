using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.App.Pages.Categories;

public partial class Index : ComponentBase
{
    #region Properties

    private List<Category> Categories { get; set; } = new();

    #endregion

    #region Services

    [Inject]
    private ICategoryHandler Handler { get; set; } = null!;
    
    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var request = new GetAllCategoriesRequest
            {
                PageNumber = 1,
                PageSize = 10
            };
            
            var response = await Handler.GetAllAsync(request);
            Categories = response.IsSuccess ? response.Data! : new();
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    #endregion
}