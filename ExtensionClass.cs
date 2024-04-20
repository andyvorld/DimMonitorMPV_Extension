using MpvNet;
using MpvNet.Windows.WinForms;
using System.Diagnostics;
using System.Windows;
using Application = System.Windows.Application;

namespace DimMonitorMPVExtension;

public class Extension : IExtension
{
    public List<Overlay> Overlays { get; private init; } = new List<Overlay>();

    public MainPlayer Player { get; private init; }
    public MainForm MainForm { get; private init; }

    public Extension()
    {
        Player = Global.Player;
        Player.ObservePropertyBool("fullscreen", FullscreenChange);

        MainForm = MainForm.Instance!;

        MainForm.MouseEnter += (_, _) => { MainFormFocusChanged(true); };
        MainForm.MouseLeave += (_, _) => { MainFormFocusChanged(false); };
    }

    private void MainFormFocusChanged(bool isFocus)
    {
        Debug.WriteLine($"Focus Changed {isFocus}");

        foreach (var overlay in Overlays)
        {
            if (isFocus)
                overlay.Show();
            else
                overlay.Hide();
        }
    }

    private void FullscreenChange(bool isFullScreen)
    {
        Debug.WriteLine($"Fullscreen: {isFullScreen}");

        if (isFullScreen)
        {
            WpfScreen activeScreen = WpfScreen.GetScreenFrom(MainForm);

            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var (i , screen) in WpfScreen.AllScreens().EnumerateWithIndex())
                {
                    if (activeScreen == screen)
                    {
                        Debug.WriteLine($"Active screen is {i}");
                        continue;
                    }

                    Overlay overlay = new()
                    {
                        Left = screen.Bounds.Left + 1,
                        Top = screen.Bounds.Top + 1,
                        WindowStartupLocation = WindowStartupLocation.Manual,
                    };

                    overlay.Show();
                    overlay.WindowState = WindowState.Maximized;

                    Overlays.Add(overlay);
                }

                _ = MainForm.Focus();
            });
        }
        else
        {
            Application.Current.Dispatcher.Invoke(() => {
                foreach (var overlay in Overlays)
                {
                    overlay.Close();
                }

                Overlays.Clear();
            });
        }
    }
}