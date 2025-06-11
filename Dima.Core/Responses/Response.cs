using System.Text.Json.Serialization;

namespace Dima.Core.Responses;

public class Response<T>
{
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    private readonly int _code;
    
    [JsonIgnore]
    public bool IsSuccess => _code is >= 200 and <= 299;

    public Response(T? data, int code = 200, string message = "")
    {
        Data = data;
        Message = message;
        _code = code;
    }

    [JsonConstructor]
    public Response()
    {
        _code = 200;
    }
}