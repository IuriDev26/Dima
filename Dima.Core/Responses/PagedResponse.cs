using System.Text.Json.Serialization;

namespace Dima.Core.Responses;

public class PagedResponse<T> : Response<T>
{
    public int CurrentPage { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    [JsonConstructor]
    public PagedResponse(T? data, int currentPage, int pageSize, int totalCount) : base(data)
    {
        Data = data;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public PagedResponse(T? data, int code = 200, string? message = null) : base(data, code, message)
    {
    }
}