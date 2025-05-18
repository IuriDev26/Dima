using Dima.App.Security;
using Dima.Core.Handlers;
using Dima.Core.Requests.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Dima.App.Pages.Identity;

public partial class Register : ComponentBase
{

    #region DependencyInjection

    [Inject]
    public IIdentityHandler IdentityHandler { get; set; } = null!;
    
    [Inject]
    public NavigationManager Navigator { get; set; } = null!;
    
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    
    [Inject]
    public ICookieAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    #endregion   
    
    #region Properties
    
    public RegisterRequest NewUser { get; set; } = new();
    
    public bool IsLoading { get; set; }
    
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

    public async Task CreateNewUserAsync()
    {
        try
        {
            IsLoading = true;

            var response = await IdentityHandler.RegisterAsync(NewUser);

            if (response.IsSuccess)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                AuthenticationStateProvider.NotifyAuthenticationStateChanged();
                Snackbar.Add(response.Message, Severity.Success);
                Navigator.NavigateTo("/");
            }
            else
            {
                Snackbar.Add(response.Message, Severity.Error);
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