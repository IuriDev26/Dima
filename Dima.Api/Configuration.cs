using System.Reflection.Metadata;

namespace Dima.Api;

public static class Configuration
{
    public static string ConnectionString = string.Empty;
    public const string CorsPolicyName = "wasm";
    public static string FrontEndUrl = string.Empty;
    public static string BackEndUrl = string.Empty;
    public const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 25;
}