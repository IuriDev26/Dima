using Dima.Core.Models;
using Microsoft.AspNetCore.Components;

namespace Dima.App.Pages.Orders;

public partial class Index : ComponentBase
{

    #region Properties

    public bool IsLoading { get; set; }
    public List<Order> Orders { get; set; }

    #endregion
    
    
}