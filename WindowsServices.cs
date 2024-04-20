using System.Runtime.InteropServices;

namespace DimMonitorMPVExtension;

public static class WindowsServices
{
    public const int WS_EX_TRANSPARENT = 0x00000020;
    public const int GWL_EX_STYLE = (-20);
    public const int WS_EX_APPWINDOW = 0x00040000, WS_EX_TOOLWINDOW = 0x00000080;


    [DllImport("user32.dll")]
    public static extern int GetWindowLong(IntPtr hwnd, int index);

    [DllImport("user32.dll")]
    public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);
}
