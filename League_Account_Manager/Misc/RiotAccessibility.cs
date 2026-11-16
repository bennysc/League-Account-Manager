using System.Runtime.InteropServices;

namespace League_Account_Manager.Misc;

internal static class RiotAccessibility
{
    // Chromium enables its accessibility tree on demand. Probe its documented
    // accessibility object before searching the renderer for sign-in controls.
    internal static void Enable(IntPtr window)
    {
        SendMessageTimeout(window, 0x003D, UIntPtr.Zero, new IntPtr(1), 2, 500, out _);
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SendMessageTimeout(IntPtr window, uint message,
        UIntPtr wParam, IntPtr lParam, uint flags, uint timeout, out UIntPtr result);
}
