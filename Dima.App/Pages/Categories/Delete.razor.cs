using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.App.Pages.Categories;

public partial class Delete : ComponentBase
{
    #region Properties

    [Parameter]
    public long Id { get; set; }
    private DeleteCategoryRequest Category { get; set; } = new();
    
    private bool IsLoading { get; set; }

    #endregion

    #region Services

    [Inject] 
    private ICategoryHandler Handler { get; set; } = null!;
    
    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;
    
    [Inject]
    private NavigationManager Navigator { get; set; } = null!;
    
    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    #endregion

    #region Methods

    private async Task DeleteCategory()
    {
        try
        {
            IsLoading = true;

            var response = await Handler.DeleteAsync(Category);
            var severity = response.IsSuccess ? Severity.Success : Severity.Error;

            Navigator.NavigateTo("/categories");
            Snackbar.Add(response.Message, severity);

        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    private async Task OnDeleteClickAsync()
    {
        var userChoice = await DialogService.ShowMessageBox("Atenção"
            , $"Deseja realemente excluir a categoria: {Category.Title}?",
            yesText: "Sim", noText: "Não");

        if (userChoice is not null && userChoice == true)
            await DeleteCategory();
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
        finally
        {
            IsLoading = false;
        }
    }

    #endregion
}