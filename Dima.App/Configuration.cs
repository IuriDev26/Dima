using MudBlazor;
using MudBlazor.Utilities;

namespace Dima.App;

public static class Configuration
{
    public const string HttpClientName = "Dima";
    public static string BackendUrl = string.Empty;
    public static int DefaultPageNumber = 1;
    public static int DefaultPageSize = 25;
    
    public static MudTheme Theme = new()
    {
        Palette = new PaletteLight
        {
            Primary = new MudColor("#1EFA2D"),
            PrimaryContrastText = new MudColor("#000000"),
            Secondary = Colors.LightGreen.Darken3,
            Background = Colors.Grey.Lighten4,
            AppbarBackground = new MudColor("#1EFA2D"),
            AppbarText = Colors.Shades.Black,
            TextPrimary = Colors.Shades.Black,
            DrawerText = Colors.Shades.White,
            DrawerBackground = Colors.Green.Darken4
        },
        PaletteDark = new PaletteDark
        {
            Primary = Colors.LightGreen.Accent3,
            Secondary = Colors.LightGreen.Darken3,
            // Background = Colors.LightGreen.Darken4,
            AppbarBackground = Colors.LightGreen.Accent3,
            AppbarText = Colors.Shades.Black,
            PrimaryContrastText = new MudColor("#000000")
        }
        
    };
}