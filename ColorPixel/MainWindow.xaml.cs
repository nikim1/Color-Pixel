using System;
using System.Windows;
using System.Drawing;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using System.Runtime.InteropServices;

namespace ColorPixel
{
    public partial class MainWindow : Window
    {
        private const UInt32 MOUSEEVENT_LEFTDOWN = 0x0002;
        private const UInt32 MOUSEEVENT_LEFTUP = 0x0004;

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInf);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Click()
        {
            mouse_event(MOUSEEVENT_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENT_LEFTUP, 0, 0, 0, 0);
        }

        private void OnButtonSearchClick(object sender, RoutedEventArgs e)
        {
            string inputHexColor = TextBoxHexColor.Text;
            SearchPixel(inputHexColor);
        }

        private void DoubleClickAtPosition(int posX, int posY)
        {
            SetCursorPos(posX, posY);
            Click();
            System.Threading.Thread.Sleep(250);
            Click();
        }

        private bool SearchPixel(string hexcode)
        {
            Bitmap bitmap = new Bitmap(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);

            Graphics graphics = Graphics.FromImage(bitmap as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);

            Color desiredPixelColor = ColorTranslator.FromHtml(hexcode);

            for (int i = 0; i < SystemInformation.VirtualScreen.Width; i++)
            {
                for (int j = 0; j < SystemInformation.VirtualScreen.Height; j++)
                {
                    Color curretnPixelColor = bitmap.GetPixel(i, j);

                    if (desiredPixelColor == curretnPixelColor)
                    {
                        MessageBox.Show(String.Format("Found Pixel at {0},{1} - Now set mouse courser", i, j));

                        DoubleClickAtPosition(i, j);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
