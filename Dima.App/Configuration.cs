using MudBlazor;

namespace Dima.App;

public static class Configuration
{
    public static MudTheme Theme = new()
    {
        Palette = new PaletteLight()
        {
            Primary = new("#1EFA2D"),
            Secondary = Colors.LightGreen.Darken3,
            AppbarBackground = new("#1EFA2D"),
            AppbarText = Colors.Shades.Black,
            TextPrimary = Colors.Shades.Black,
            DrawerText = Colors.Shades.Black,
            DrawerBackground = Colors.Shades.Black,
            Background = Colors.Grey.Lighten4,
        },
        PaletteDark = new PaletteDark()
        {
            Primary = Colors.LightGreen.Accent3,
            Secondary = Colors.LightGreen.Darken3,
            AppbarBackground = Colors.LightGreen.Accent3,
            AppbarText = Colors.Shades.Black
        }
        
    };
}