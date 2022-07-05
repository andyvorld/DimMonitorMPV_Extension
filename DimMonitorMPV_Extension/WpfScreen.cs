using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace DimMonitorMPVExtension
{
    public class WpfScreen : IEquatable<WpfScreen>
    {
        public static IEnumerable<WpfScreen> AllScreens()
        {
            foreach (Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                yield return new WpfScreen(screen);
            }
        }

        public static WpfScreen GetScreenFrom(Window window)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
            Screen screen = System.Windows.Forms.Screen.FromHandle(windowInteropHelper.Handle);
            WpfScreen wpfScreen = new WpfScreen(screen);
            return wpfScreen;
        }

        public static WpfScreen GetScreenFrom(Form form)
        {
            Rectangle rect = new Rectangle
            {
                X = form.Bounds.X,
                Y = form.Bounds.Y,
                Width = form.Bounds.Width,
                Height = form.Bounds.Height
            };

            Screen screen = System.Windows.Forms.Screen.FromRectangle(rect);

            return new WpfScreen(screen);
        }

        public static WpfScreen GetScreenFrom(System.Drawing.Point point)
        {
            int x = point.X;
            int y = point.Y;

            // are x,y device-independent-pixels ??
            System.Drawing.Point drawingPoint = new System.Drawing.Point(x, y);
            Screen screen = System.Windows.Forms.Screen.FromPoint(drawingPoint);
            WpfScreen wpfScreen = new WpfScreen(screen);

            return wpfScreen;
        }

        public static WpfScreen Primary
        {
            get { return new WpfScreen(System.Windows.Forms.Screen.PrimaryScreen); }
        }

        private readonly Screen screen;

        internal WpfScreen(System.Windows.Forms.Screen screen)
        {
            this.screen = screen;
        }

        public Rect DeviceBounds
        {
            get { return this.GetRect(this.screen.Bounds); }
        }

        public Rect WorkingArea
        {
            get { return this.GetRect(this.screen.WorkingArea); }
        }

        private Rect GetRect(Rectangle value)
        {
            // should x, y, width, height be device-independent-pixels ??
            return new Rect
            {
                X = value.X,
                Y = value.Y,
                Width = value.Width,
                Height = value.Height
            };
        }

        public override bool Equals(object obj) => this.Equals(obj as WpfScreen);
        public bool Equals(WpfScreen other)
        {
            return (this.screen.Bounds.X == other.screen.Bounds.X) &&
                    (this.screen.Bounds.Top == other.screen.Bounds.Top);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 13;
                hash = (hash * 7) + this.screen.Bounds.X.GetHashCode();
                hash = (hash * 7) + this.screen.Bounds.Top.GetHashCode();
                return hash;
            }
        }

        public static bool operator==(WpfScreen a, WpfScreen b)
        {
            return a.Equals(b);
        }

        public static bool operator!=(WpfScreen a, WpfScreen b)
        {
            return !a.Equals(b);
        }

        public bool IsPrimary
        {
            get { return this.screen.Primary; }
        }

        public string DeviceName
        {
            get { return this.screen.DeviceName; }
        }
    }
}
