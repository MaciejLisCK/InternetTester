using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace InternetTester.BusinessLogic.DesktopDrawer
{
    public class DesktopContextWraper : IDisposable
    {
        Graphics _graphics;
        IntPtr _desktopPtr;

        public Graphics Context { get; private set; }

        public DesktopContextWraper()
        {
            _desktopPtr = GetDC(IntPtr.Zero);
            Context = Graphics.FromHdc(_desktopPtr);
        }

        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);

        public void Dispose()
        {
            Context.Dispose();
            ReleaseDC(IntPtr.Zero, _desktopPtr);
        }
    }
}
