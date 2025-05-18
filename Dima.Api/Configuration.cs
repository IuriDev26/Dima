using System.Reflection.Metadata;

namespace Dima.Api;

public static class Configuration
{
    public static string ConnectionString = string.Empty;
    public static string CorsPolicyName = "wasm";
    public static string FrontEndUrl = string.Empty;
    public static string BackEndUrl = string.Empty;
    public static int DefaultPageNumber = 1;
    public static int DefaultPageSize = 25;
}