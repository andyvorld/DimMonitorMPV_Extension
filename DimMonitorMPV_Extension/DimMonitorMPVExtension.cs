using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using mpvnet;

namespace DimMonitorMPVExtension
{
    [Export(typeof(IExtension))]
    public class DimMonitorMPVExtension : IExtension
    {
        readonly List<Overlay> overlays = new List<Overlay>(5);
        readonly MainForm MainForm;
        readonly CorePlayer Core;

        public DimMonitorMPVExtension() // plugin initialization
        {
            Debug.WriteLine("DimMonitorMPVExtension init");

            MainForm = MainForm.Instance;
            Core = Global.Core;

            Core.ObservePropertyBool("fullscreen", FullscreenChange);

            MainForm.MouseEnter += MainForm_MouseEnter;
            MainForm.MouseLeave += MainForm_MouseLeave;

            foreach (WpfScreen screen in WpfScreen.AllScreens())
            {
                double x = screen.DeviceBounds.Left;
                double y = screen.DeviceBounds.Top;

                Debug.WriteLine($"{x}, {y}");
            }
        }

        private void MainForm_MouseLeave(object sender, EventArgs e)
        {
            MainForm_FocusChanged(false);
        }

        private void MainForm_MouseEnter(object sender, EventArgs e)
        {
            MainForm_FocusChanged(true);
        }

        private void MainForm_FocusChanged(bool focus)
        {
            Debug.WriteLine($"Focus Changed {focus}");

            foreach (Overlay overlay in overlays)
            {
                if (focus)
                {
                    overlay.Show();
                }
                else
                {
                    overlay.Hide();
                }
            }
        }

        private void FullscreenChange(bool isFullScreen)
        {
            Debug.WriteLine($"Fullscreen: {isFullScreen}");

            int x = MainForm.Instance.Left;
            int y = MainForm.Instance.Top;

            if (isFullScreen)
            {
                WpfScreen activeScreen = WpfScreen.GetScreenFrom(MainForm);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    int i = 0;
                    foreach (WpfScreen screen in WpfScreen.AllScreens())
                    {
                        i++;
                        if (activeScreen == screen)
                        {
                            Debug.WriteLine($"Active screen is {i}");
                            continue;
                        }

                            Overlay overlay = new Overlay
                            {
                                Left = screen.DeviceBounds.Left + 1,
                                Top = screen.DeviceBounds.Top - 1,
                                WindowStartupLocation = WindowStartupLocation.Manual
                            };

                            overlay.Show();
                            overlay.WindowState = WindowState.Maximized;

                            overlays.Add(overlay);

                    }

                    _ = MainForm.Focus();
                });
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (Overlay overlay in overlays)
                    {
                        overlay.Close();
                    }
                });

                overlays.Clear();
            }
        }
    }
}
