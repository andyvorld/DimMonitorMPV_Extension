using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Animation;

using static DimMonitorMPVExtension.WindowsServices;
using Color = System.Windows.Media.Color;

namespace DimMonitorMPVExtension;

public partial class Overlay : Window
{
    private const double OPACITY_MAX = 0.7;
    private const double OPACITY_PERIOD = 1500;

    public Overlay()
    {
        InitializeComponent();

        ColorAnimation colorChangeAnimation = new()
        {
            From = Color.FromArgb(0x00, 0, 0, 0),
            To = Color.FromArgb((byte)(255 * OPACITY_MAX), 0, 0, 0),
            Duration = new Duration(TimeSpan.FromMilliseconds(OPACITY_PERIOD)),
            FillBehavior = FillBehavior.HoldEnd
        };

        PropertyPath colorTargetPath = new("(Grid.Background).(SolidColorBrush.Color)");
        Storyboard CellBackgroundChangeStory = new();
        Storyboard.SetTarget(colorChangeAnimation, FillGrid);
        Storyboard.SetTargetProperty(colorChangeAnimation, colorTargetPath);
        CellBackgroundChangeStory.Children.Add(colorChangeAnimation);
        CellBackgroundChangeStory.Begin();
    }

    private void Overlay_Loaded(object sender, RoutedEventArgs e)
    {
        var helper = new WindowInteropHelper(this).Handle;

        //Performing some magic to hide the form from Alt+Tab
        _ = SetWindowLong(helper, GWL_EX_STYLE, (GetWindowLong(helper, GWL_EX_STYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
    }
}
