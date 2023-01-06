using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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

using static DimMonitorMPVExtension.WindowsServices;

namespace DimMonitorMPVExtension
{
    /// <summary>
    /// Interaction logic for Overlay.xaml
    /// </summary>
    /// 
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

        private void Overlay_Loaded(object sender, RoutedEventArgs e)
        {
            var helper = new WindowInteropHelper(this).Handle;
            //Performing some magic to hide the form from Alt+Tab
            SetWindowLong(helper, GWL_EX_STYLE, (GetWindowLong(helper, GWL_EX_STYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
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

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            timer.Stop();
        }
    }
}
