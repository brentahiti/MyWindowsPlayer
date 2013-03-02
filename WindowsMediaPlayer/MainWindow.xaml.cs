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

        public MainWindow()
        {
            InitializeComponent();
            this.Action = new Modele();
        }

        private void ButtonRead(object sender, RoutedEventArgs e)
        {
            var    tmp = Action.ModeleRead();

            if (tmp == eRead.Pause)
                this.LectureButton.Content = "Lecture";
            else if (tmp == eRead.Play)
                this.LectureButton.Content = "Pause";
        }

        private void ButtonStop(object sender, RoutedEventArgs e)
        {
            var tmp = Action.ModeleStop();

            if (tmp == eStop.Stop)
                this.LectureButton.Content = "Lecture";
        }

        private void ButtonPath(object sender, RoutedEventArgs e)
        {
            Action.ModelePath();
        }

        private void ValueSound(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Action.ModeleVolume(this.Volume.Value);
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
        }

        public void ModeleVolume(Double Value)
        {
            this.player.Volume = Value / 10;
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
