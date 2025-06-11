using Microsoft.AspNetCore.Components;

namespace Dima.App.Components;

public partial class LoadingComponent : ComponentBase
{
    #region Parameters

    public string Message { get; set; } = string.Empty;

    #endregion
}