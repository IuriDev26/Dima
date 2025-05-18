using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.App.Pages.Categories;

public partial class Create : ComponentBase
{
    #region Properties

    private CreateCategoryRequest Category { get; set; } = new();
    
    private bool IsLoading { get; set; }

    #endregion

    #region Services

    [Inject] 
    private ICategoryHandler Handler { get; set; } = null!;
    
    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;
    
    [Inject]
    private NavigationManager Navigator { get; set; } = null!;

    #endregion

    #region Methods

    private async Task CreateCategory()
    {
        try
        {
            IsLoading = true;

            var response = await Handler.CreateAsync(Category);
            var severity = response.IsSuccess ? Severity.Success : Severity.Error;

            Navigator.NavigateTo("/categories");
            Snackbar.Add(response.Message, severity);

        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    #endregion
}