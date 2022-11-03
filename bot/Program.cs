using System.Drawing;
using System.Runtime.InteropServices;
using WindowsInput.Native;
using WindowsInput;
namespace Bot
{
    class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindowDC(IntPtr window);
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern uint GetPixel(IntPtr dc, int x, int y);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int ReleaseDC(IntPtr window, IntPtr dc);

        public static Color GetColorAt(int x, int y)
        {
            IntPtr desk = GetDesktopWindow();
            IntPtr dc = GetWindowDC(desk);
            int a = (int)GetPixel(dc, x, y);
            ReleaseDC(desk, dc);

            return Color.FromArgb(255, (a >> 0) & 0xff, (a >> 8) & 0xff, (a >> 16) & 0xff);
        }
        static void Main()
        {
            InputSimulator sim = new InputSimulator();
            string lastButton = "";
            while (true)
            {
                Color leftPixel = GetColorAt(1254, 983);
                Color rightPixel = GetColorAt(1305, 983);

                //берём с левой стороны пиксель
                if (leftPixel.R == 161 && leftPixel.G == 116 && leftPixel.B == 56)
                {
                    lastButton = "RIGHT";
                    sim.Keyboard.KeyPress(VirtualKeyCode.RIGHT);
                } else if (rightPixel.R == 161 && rightPixel.G == 116 && rightPixel.B == 56)
                {
                    lastButton = "LEFT";
                    sim.Keyboard.KeyPress(VirtualKeyCode.LEFT);
                } else
                {
                    if (lastButton == "RIGHT")
                    {
                        sim.Keyboard.KeyPress(VirtualKeyCode.RIGHT);
                    }
                    if (lastButton == "LEFT")
                    {
                        sim.Keyboard.KeyPress(VirtualKeyCode.LEFT);
                    }
                }
                Console.WriteLine(lastButton);
                Thread.Sleep(80);
            }
        }
    }
}