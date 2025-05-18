using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.App.Pages.Categories;

public partial class Update : ComponentBase
{
    #region Properties

    [Parameter]
    public long Id { get; set; }
    private UpdateCategoryRequest Category { get; set; } = new();
    
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

    private async Task UpdateCategory()
    {
        try
        {
            IsLoading = true;

            var response = await Handler.UpdateAsync(Category);
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

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        try
        {
            IsLoading = true;

            var request = new GetCategoryByIdRequest(Id);

            var response = await Handler.GetByIdAsync(request);

            if (!response.IsSuccess)
            {
                Snackbar.Add(response.Message, Severity.Error);
                Navigator.NavigateTo("/categories");
            }

            var category = response.Data;

            Category.Description = category!.Description ?? string.Empty;
            Category.Id = category!.Id;
            Category.Title = category.Title;

        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    #endregion
}