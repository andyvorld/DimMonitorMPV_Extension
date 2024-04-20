using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace DimMonitorMPVExtension;

public readonly record struct WpfScreen
{
    public static IEnumerable<WpfScreen> AllScreens()
    {
        foreach (Screen screen in Screen.AllScreens)
        {
            yield return new WpfScreen(screen);
        }
    }

    public static WpfScreen GetScreenFrom(Form form)
    {
        Rectangle rect = new
        (
            form.Bounds.X,
            form.Bounds.Y,
            form.Bounds.Width,
            form.Bounds.Height
        );

        return new(Screen.FromRectangle(rect));
    }

    public Rect Bounds { get; private init; }

    private WpfScreen(Rect bounds)
    {
        Bounds = bounds;
    }

    private WpfScreen(Screen screen) : this(screen.Bounds.ToRect()) { }

    public bool Equals(WpfScreen other) => 
        (Bounds.X == other.Bounds.X) &&
        (Bounds.Top == other.Bounds.Top);

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 13;
            hash = (hash * 7) + Bounds.X.GetHashCode();
            hash = (hash * 7) + Bounds.Top.GetHashCode();
            return hash;
        }
    }
}
