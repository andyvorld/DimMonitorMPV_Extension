using System.Drawing;
using System.Windows;

namespace DimMonitorMPVExtension;

public static class ExtensionUtils
{
    public static Rect ToRect(this Rectangle rectangle) => new(
        rectangle.X,
        rectangle.Y,
        rectangle.Width,
        rectangle.Height
    );

    public static IEnumerable<(int, T)> EnumerateWithIndex<T>(this IEnumerable<T> enumerable)
    {
        int i = 0;
        foreach (var item in enumerable)
        {
            yield return (i, item);
            i++;
        }
    }
}
