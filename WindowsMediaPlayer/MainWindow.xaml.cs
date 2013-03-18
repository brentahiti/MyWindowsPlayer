using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace WindowsMediaPlayer
{

    public enum eRead {Nothing, Play, Pause};
    public enum eStop { Nothing, Stop };
    public enum ePath { Nothing, CorrectPath };

    /// <summary>
    /// View of WMP
    /// </summary>
    public partial class MainWindow : Window
    {
        protected Modele Action;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private const int WM_SYSCOMMAND = 0x112;
        private HwndSource hwndSource;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public enum ResizeDirection
        {
            Left = 1,
            Right = 2,
            Top = 3,
            TopLeft = 4,
            TopRight = 5,
            Bottom = 6,
            BottomLeft = 7,
            BottomRight = 8
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Action = new Modele();
            this.hwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
        }

        private void ResizeWindow(ResizeDirection d)
        {
            SendMessage(hwndSource.Handle, WM_SYSCOMMAND, (IntPtr)(61440 + d), IntPtr.Zero);
        }

        private void TitleBarGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ReleaseCapture();
            SendMessage(new WindowInteropHelper(this).Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Maximized)
                this.WindowState = System.Windows.WindowState.Normal;
            else
                this.WindowState = System.Windows.WindowState.Maximized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ResetCursor(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
                this.Cursor = Cursors.Arrow;
        }

        private void Resize(object sender, MouseEventArgs e)
        {
            Rectangle clickedRectangle = sender as Rectangle;
            switch (clickedRectangle.Name)
            {
                case "top":
                    this.Cursor = Cursors.SizeNS;
                    ResizeWindow(ResizeDirection.Top);
                    break;
                case "bottom":
                    this.Cursor = Cursors.SizeNS;
                    ResizeWindow(ResizeDirection.Bottom);
                    break;
                case "left":
                case "left1":
                    this.Cursor = Cursors.SizeWE;
                    ResizeWindow(ResizeDirection.Left);
                    break;
                case "right":
                case "right1":
                    this.Cursor = Cursors.SizeWE;
                    ResizeWindow(ResizeDirection.Right);
                    break;
                case "topLeft":
                    this.Cursor = Cursors.SizeNWSE;
                    ResizeWindow(ResizeDirection.TopLeft);
                    break;
                case "topRight":
                    this.Cursor = Cursors.SizeNESW;
                    ResizeWindow(ResizeDirection.TopRight);
                    break;
                case "bottomLeft":
                    this.Cursor = Cursors.SizeNESW;
                    ResizeWindow(ResizeDirection.BottomLeft);
                    break;
                case "bottomRight":
                    this.Cursor = Cursors.SizeNWSE;
                    ResizeWindow(ResizeDirection.BottomRight);
                    break;
                default:
                    break;
            }

        }

        private void DisplayResizeCursor(object sender, MouseEventArgs e)
        {
            Rectangle hoveredRectangle = sender as Rectangle;
            Console.WriteLine(hoveredRectangle.Name);
            switch (hoveredRectangle.Name)
            {
                case "top":
                case "bottom":
                    this.Cursor = Cursors.SizeNS;
                    break;
                case "left":
                case "left1":
                    this.Cursor = Cursors.SizeWE;
                    break;
                case "right":
                case "right1":
                    this.Cursor = Cursors.SizeWE;
                    break;
                case "topLeft":
                case "bottomRight":
                    this.Cursor = Cursors.SizeNWSE;
                    break;
                case "topRight":
                case "bottomLeft":
                    this.Cursor = Cursors.SizeNESW;
                    break;
                default:
                    break;
            }
        }

        private void full_Click(object sender, RoutedEventArgs e)
        {
            if (!this.Topmost)
            {
                this.Topmost = true;
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.Topmost = true;
            }
        }
    }

    /// <summary>
    /// Modele of WMP
    /// </summary>
    public class Modele
    {
        private MediaPlayer player;
        private string path = null;
        private bool isReading = false;
        private bool pause = false;

        public Modele()
        {
            this.player = new MediaPlayer();
            this.player.Volume = 0.0;
        }

        public ePath ModelePath()
        {
            OpenFileDialog windowsDial = new OpenFileDialog();
            windowsDial.FileName = "File";
            windowsDial.DefaultExt = ".avi";
            windowsDial.Filter = "Music file (.mp3)|*.mp3| " + "Video File (.avi)|*.avi";

            Nullable<bool> result = windowsDial.ShowDialog();
            if (result == true)
            {
                this.path = windowsDial.FileName;
                this.isReading = false;
                this.pause = false;
                return ePath.CorrectPath;
            }
            return ePath.Nothing;
        }

        public eStop ModeleStop()
        {
            if (this.pause == true || this.isReading == true)
            {
                this.isReading = false;
                this.pause = false;
                this.player.Stop();
                return eStop.Stop;
            }
            return eStop.Nothing;
        }

        public eRead ModeleRead()
        {
            if (this.path != null)
            {

                if (this.isReading == true && this.pause == false)
                {
                    this.player.Pause();
                    this.pause = true;
                    this.isReading = false;
                    return eRead.Pause;
                }
                if (this.isReading == false && this.pause == false)
                {
                    this.player.Open(new System.Uri(this.path));
                }
                this.player.Play();
                this.isReading = true;
                this.pause = false;
                return eRead.Play;
            }
            return eRead.Nothing;
        }
    }
}
