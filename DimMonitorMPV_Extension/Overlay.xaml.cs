using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DimMonitorMPVExtension
{
    /// <summary>
    /// Interaction logic for Overlay.xaml
    /// </summary>
    /// 

    //public static class WindowsServices
    //{
    //    const int WS_EX_TRANSPARENT = 0x00000020;
    //    const int GWL_EXSTYLE = (-20);

    //    [DllImport("user32.dll")]
    //    static extern int GetWindowLong(IntPtr hwnd, int index);

    //    [DllImport("user32.dll")]
    //    static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

    //    public static void SetWindowExTransparent(IntPtr hwnd)
    //    {
    //        var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
    //        SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
    //    }
    //}

    public partial class Overlay : Window
    {
        //private const int HITTEST_M = 0x84;

        private const double OPACITY_DELTA = 0.005;
        private const double OPACITY_MAX = 0.7;
        private const double OPACITY_PERIOD = OPACITY_MAX / OPACITY_DELTA;

        private readonly DispatcherTimer timer;
        public Overlay()
        {
            InitializeComponent();

            Debug.WriteLine($"Fade time {OPACITY_PERIOD} ms");

            timer = new DispatcherTimer(DispatcherPriority.Send)
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.Fill.Opacity += OPACITY_DELTA;
            //Debug.WriteLine($"{this.Fill.Opacity}");

            if (this.Fill.Opacity >= OPACITY_MAX)
            {
                this.Fill.Opacity = OPACITY_MAX;
                timer.Stop();
            }
        }

        //protected override void OnInitialized(EventArgs e)
        //{
        //    base.OnInitialized(e);
        //    IntPtr hwnd = new WindowInteropHelper(this).Handle;
        //    WindowsServices.SetWindowExTransparent(hwnd);
        //}

        //protected override void OnSourceInitialized(EventArgs e)
        //{
        //    base.OnSourceInitialized(e);
        //    HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
        //    source.AddHook(WndProc);
        //}

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            timer.Stop();
        }

        //private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        //{
        //    //if (msg == HITTEST_M)
        //    //{
        //    //    Debug.WriteLine("HIT TEST");
        //    //    handled = true;
        //    //    return (IntPtr)(-1);
        //    //}

        //    return IntPtr.Zero;
        //}
    }
}
