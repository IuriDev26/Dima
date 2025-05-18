using Dima.App.Security;
using Dima.Core.Handlers;
using Dima.Core.Requests.Identity;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.App.Pages.Identity;

public partial class Login : ComponentBase
{
    #region DependecyInjection

    [Inject]
    private IIdentityHandler IdentityHandler { get; set; } = null!;
    
    [Inject]
    private NavigationManager Navigator { get; set; } = null!;
    
    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;
    
    [Inject]
    private ICookieAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    #endregion

    #region Properties

    private LoginRequest User { get; set; } = new();
    
    private bool IsLoading { get; set; }

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User; 
        
        if (user.Identity is not null && user.Identity.IsAuthenticated)
            Navigator.NavigateTo("/");
            
    }

    #endregion

    #region Methods

    public async Task LoginAsync()
    {
        try
        {
            IsLoading = true;
            
            var response = await IdentityHandler.LoginAsync(User);

            if (response.IsSuccess)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                AuthenticationStateProvider.NotifyAuthenticationStateChanged();
                Navigator.NavigateTo("/");
            }
            else
                Snackbar.Add(response.Message, Severity.Error);

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