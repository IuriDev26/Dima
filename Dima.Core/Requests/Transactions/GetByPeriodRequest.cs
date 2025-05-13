namespace Dima.Core.Requests.Transactions;

public class GetByPeriodRequest : PagedRequest
{
    public DateTime? InitialInterval { get; set; }
    public DateTime? FinalInterval { get; set; }
}