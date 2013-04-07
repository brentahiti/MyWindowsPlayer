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
using System.Timers;

namespace WindowsMediaPlayer
{
    /// <summary>
    /// View of WMP
    /// </summary>
    public partial class MainWindow : Window
    {
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

        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        private struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        private List<double> _colsDefinition = new List<double> ();
        private List<double> _rowsDefinition = new List<double> ();
        private Brush BgNormal;

        private bool isFullScreen = false;
        private double layoutWidth = -1;
        private double bottomHeight = -1;

        private Timer timer;

        public bool IsHidden { get; private set; }

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
            if (this.LayoutDefinition.ColumnDefinitions[2].ActualWidth > 0)
                this.LayoutDefinition.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Pixel);

            this.hwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;

            this.timer = new Timer();
            this.timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            this.timer.Interval = 500;
            this.timer.Enabled = true;
            this.timer.Start();

            var row = this.MainLayoutGrid.ColumnDefinitions;
            for (var i = 0; i < row.Count(); i++)
            {
                var current = row.ElementAt(i);
                if (current.Width.GridUnitType == GridUnitType.Pixel)
                    this._rowsDefinition.Add(current.Width.Value);
            }

            var col = this.MainLayoutGrid.RowDefinitions;
            for (var i = 0; i < col.Count(); i++)
            {
                var current = col.ElementAt(i);
                if (current.Height.GridUnitType == GridUnitType.Pixel)
                    this._colsDefinition.Add(current.Height.Value);
            }

            this.BgNormal = this.MainContent.Background;

        }

        private void ResizeWindow(ResizeDirection d)
        {
            if (!this.isFullScreen)
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
                this.Cursor = System.Windows.Input.Cursors.Arrow;
        }

        private void Resize(object sender, MouseEventArgs e)
        {
            Rectangle clickedRectangle = sender as Rectangle;
            switch (clickedRectangle.Name)
            {
                case "top":
                    this.Cursor = System.Windows.Input.Cursors.SizeNS;
                    ResizeWindow(ResizeDirection.Top);
                    break;
                case "bottom":
                    this.Cursor = System.Windows.Input.Cursors.SizeNS;
                    ResizeWindow(ResizeDirection.Bottom);
                    break;
                case "left":
                case "left1":
                    this.Cursor = System.Windows.Input.Cursors.SizeWE;
                    ResizeWindow(ResizeDirection.Left);
                    break;
                case "right":
                case "right1":
                    this.Cursor = System.Windows.Input.Cursors.SizeWE;
                    ResizeWindow(ResizeDirection.Right);
                    break;
                case "topLeft":
                    this.Cursor = System.Windows.Input.Cursors.SizeNWSE;
                    ResizeWindow(ResizeDirection.TopLeft);
                    break;
                case "topRight":
                    this.Cursor = System.Windows.Input.Cursors.SizeNESW;
                    ResizeWindow(ResizeDirection.TopRight);
                    break;
                case "bottomLeft":
                    this.Cursor = System.Windows.Input.Cursors.SizeNESW;
                    ResizeWindow(ResizeDirection.BottomLeft);
                    break;
                case "bottomRight":
                    this.Cursor = System.Windows.Input.Cursors.SizeNWSE;
                    ResizeWindow(ResizeDirection.BottomRight);
                    break;
                default:
                    break;
            }

        }

        private void DisplayResizeCursor(object sender, MouseEventArgs e)
        {
            Rectangle hoveredRectangle = sender as Rectangle;
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
                this.BorderFullScreen1.BorderThickness = new System.Windows.Thickness(1, 1, 0, 0);
                this.BorderFullScreen2.BorderThickness = new System.Windows.Thickness(0, 1, 1, 0);
                this.BorderFullScreen3.BorderThickness = new System.Windows.Thickness(1, 0, 0, 1);
                this.BorderFullScreen4.BorderThickness = new System.Windows.Thickness(0, 0, 1, 1);


                if (this.layoutWidth == -1)
                    this.layoutWidth = this.LayoutDefinition.ColumnDefinitions[0].ActualWidth;
                this.LayoutDefinition.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Pixel);
                this.LayoutDefinition.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Pixel);

                this.Topmost = true;
                this.isFullScreen = true;
                this.WindowState = WindowState.Maximized;
                this.MainContent.Background = Brushes.Black;

                var row = this.MainLayoutGrid.ColumnDefinitions;
                for (var i = 0; i < row.Count(); i++)
                {
                    var current = row.ElementAt(i);
                    if (current.Width.GridUnitType == GridUnitType.Pixel)
                        current.Width = new GridLength(0, GridUnitType.Pixel);
                }

                var col = this.MainLayoutGrid.RowDefinitions;
                for (var i = 0; i < col.Count(); i++)
                {
                    var current = col.ElementAt(i);
                    if (current.Height.GridUnitType == GridUnitType.Pixel)
                        current.Height = new GridLength(0, GridUnitType.Pixel);
                }
            }
            else
            {
                this.BorderFullScreen1.BorderThickness = new System.Windows.Thickness(0, 0, 1, 1);
                this.BorderFullScreen2.BorderThickness = new System.Windows.Thickness(1, 0, 0, 1);
                this.BorderFullScreen3.BorderThickness = new System.Windows.Thickness(0, 1, 1, 0);
                this.BorderFullScreen4.BorderThickness = new System.Windows.Thickness(1, 1, 0, 0);

                this.WindowState = WindowState.Normal;
                this.isFullScreen = false;
                this.Topmost = false;
                this.MainContent.Background = this.BgNormal;

                var row = this.MainLayoutGrid.ColumnDefinitions;
                int idxSave = 0;
                for (var i = 0; i < row.Count(); i++)
                {
                    var current = row.ElementAt(i);
                    if (current.Width.GridUnitType == GridUnitType.Pixel)
                        current.Width = new GridLength(this._rowsDefinition.ElementAt(idxSave++), GridUnitType.Pixel);
                }

                var col = this.MainLayoutGrid.RowDefinitions;
                idxSave = 0;
                for (var i = 0; i < col.Count(); i++)
                {
                    var current = col.ElementAt(i);
                    if (current.Height.GridUnitType == GridUnitType.Pixel)
                        current.Height = new GridLength(this._colsDefinition.ElementAt(idxSave++), GridUnitType.Pixel);
                }

            }

        }

        private void mediaElement_Drop(object sender, DragEventArgs e)
        {
            Console.WriteLine((string)((DataObject)e.Data).GetFileDropList()[0]);
        }

        private void ToggleRightListGrid(object sender, RoutedEventArgs e)
        {
            if (this.layoutWidth == -1)
                this.layoutWidth = this.LayoutDefinition.ColumnDefinitions[0].ActualWidth;
            if (this.LayoutDefinition.ColumnDefinitions[2].ActualWidth > 0)
                this.LayoutDefinition.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Pixel);
            else
            {
                if (this.LayoutDefinition.ColumnDefinitions[0].ActualWidth > 0)
                    this.LayoutDefinition.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Pixel);
                this.LayoutDefinition.ColumnDefinitions[2].Width = new GridLength(this.layoutWidth, GridUnitType.Pixel);
            }
        }

        private void TogglePlayListGrid(object sender, RoutedEventArgs e)
        {
            if (this.layoutWidth == -1)
                this.layoutWidth = this.LayoutDefinition.ColumnDefinitions[0].ActualWidth;
            if (this.LayoutDefinition.ColumnDefinitions[0].ActualWidth > 0)
                this.LayoutDefinition.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Pixel);
            else
            {
                if (this.LayoutDefinition.ColumnDefinitions[2].ActualWidth > 0)
                    this.LayoutDefinition.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Pixel);
                this.LayoutDefinition.ColumnDefinitions[0].Width = new GridLength(this.layoutWidth, GridUnitType.Pixel);
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (isFullScreen && SecondsSinceLastInput() > 2 && !IsHidden)
            {
                this.Dispatcher.Invoke(new Action(() =>
                    {
                        this.Cursor = Cursors.None;
                        if (this.bottomHeight == -1)
                            this.bottomHeight = this.MainContent.RowDefinitions[1].ActualHeight;
                        this.MainContent.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Pixel);
                    }));
                IsHidden = true;
            }
            else if (IsHidden && SecondsSinceLastInput() <= 2)
            {
                this.Dispatcher.Invoke(new Action(() =>
                    {
                        this.Cursor = Cursors.Arrow;
                        this.MainContent.RowDefinitions[1].Height = new GridLength(this.bottomHeight, GridUnitType.Pixel);
                    }));
                IsHidden = false;
            }
        }

        public static double SecondsSinceLastInput()
        {
            LASTINPUTINFO lastInPut = new LASTINPUTINFO();
            lastInPut.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
            GetLastInputInfo(ref lastInPut);

            uint idle = (uint)Environment.TickCount - lastInPut.dwTime;
            return idle / 1000.0;
        }


    }
}