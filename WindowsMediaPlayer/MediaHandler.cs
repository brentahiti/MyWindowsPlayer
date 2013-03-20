using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace WindowsMediaPlayer
{
    public enum ePlayState { Play, Pause, Stop }

    class MediaHandler
    {
        private DispatcherTimer mediaTimer;
        private ePlayState playState;
        public ePlayState PlayState
        {
            get { return this.playState; }
            set
            {
                this.playState = value;
                if (this.FileEvent != null)
                    this.FileEvent(this, new FileEventArg(this.PlayState));
            }
        }
        public MediaElement MediaPlayer { get; private set; }
        public RessourceManager RessourceManager { get; private set; }
        public Slider ProgressBar { get; private set; }
        public TimeSpan FileDuration { get; private set; }

        public event EventHandler<FileEventArg> FileEvent;
        public event EventHandler FileLoaded;
        public event EventHandler FileEnded;

        public MediaHandler(RessourceManager rm)
        {
            this.PlayState = ePlayState.Stop;

            this.MediaPlayer = new MediaElement();
            this.MediaPlayer.VerticalAlignment = VerticalAlignment.Center;
            this.MediaPlayer.Height = Double.NaN;
            this.MediaPlayer.Width = Double.NaN;
            this.MediaPlayer.LoadedBehavior = MediaState.Manual;
            this.MediaPlayer.MediaOpened += new RoutedEventHandler(OnMediaOpened);
            this.MediaPlayer.MediaEnded += new RoutedEventHandler(OnMediaEnd);

            this.ProgressBar = new Slider();
            this.ProgressBar.Value = 0.0;
            this.ProgressBar.Maximum = 1.0;
            this.ProgressBar.IsMoveToPointEnabled = true;
            this.ProgressBar.ValueChanged += new RoutedPropertyChangedEventHandler<double>(OnSliderValueChange);
            //MediaHandler.GetThumb(this.ProgressBar).DragCompleted += new DragCompletedEventHandler(OnSliderDragCompleted);

            this.RessourceManager = rm;
        }

        public void PlayFile()
        {
            if (this.RessourceManager.FileFound)
            {
                if (this.PlayState == ePlayState.Play)
                {
                    this.MediaPlayer.Pause();
                    this.PlayState = ePlayState.Pause;
                }
                else
                {

                    if (this.PlayState == ePlayState.Stop)
                    {
                        if (this.RessourceManager.playlistFound
                            && (this.RessourceManager.currentElementInPlaylist < this.RessourceManager.numberElementInPlaylist))
                        {
                            this.MediaPlayer.Source = (new System.Uri(this.RessourceManager.playlist.playList[this.RessourceManager.currentElementInPlaylist].Path, UriKind.Relative));
                            ++(this.RessourceManager.currentElementInPlaylist);
                        }
                        else
                            this.MediaPlayer.Source = (new System.Uri(this.RessourceManager.FilePath, UriKind.Relative));
                    }
                    this.MediaPlayer.Play();
                    this.PlayState = ePlayState.Play;
                }
            }
        }

        public void StopFile()
        {
            if (this.PlayState != ePlayState.Stop)
            {
                this.MediaPlayer.Stop();
                this.PlayState = ePlayState.Stop;
                this.RessourceManager.currentElementInPlaylist = 0;
            }
        }

        private static Thumb GetThumb(Slider slider)
        {
            //slider.Measure(new Size(200, 200));
            //slider.Arrange(new Rect(0, 0, 200, 200));
            //Viewbox v = new Viewbox();
            //v.Child = slider;
            //v.Measure(new System.Windows.Size(200, 200));
            //v.Arrange(new Rect(0, 0, 200, 200));
            //v.UpdateLayout();
            //RenderTargetBitmap render = new RenderTargetBitmap(200, 200, 150, 150, PixelFormats.Pbgra32);
            //render.Render(v);
            slider.ApplyTemplate();
            var track = slider.Template.FindName("PART_Track", slider) as Track;
            return track == null ? null : track.Thumb;
        }

        private void OnMediaOpened(object sender, RoutedEventArgs e)
        {
            this.FileDuration = this.MediaPlayer.NaturalDuration.TimeSpan;
            this.ProgressBar.Maximum = this.MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;

            this.mediaTimer = new DispatcherTimer();
            this.mediaTimer.Interval = TimeSpan.FromSeconds(1);
            this.mediaTimer.Tick += new EventHandler(MediaTimerTick);
            this.mediaTimer.Start();

            if (this.FileLoaded != null)
            {
                this.FileLoaded(this, new EventArgs());
            }
        }

        private void OnMediaEnd(object sender, RoutedEventArgs e)
        {
            if (this.FileEnded != null)
            {
                this.FileEnded(this, new EventArgs());
            }
        }

        private void MediaTimerTick(object sender, EventArgs e)
        {
            if (this.ProgressBar.Maximum > 0)
            {
                this.ProgressBar.Value = this.MediaPlayer.Position.TotalSeconds;
            }
        }

        private void OnSliderValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.ProgressBar.Maximum > 0)
            {
                this.MediaPlayer.Position = TimeSpan.FromSeconds(this.ProgressBar.Value);
            }
        }

        //private void OnSliderDragCompleted(object sender, DragCompletedEventArgs e)
        //{
        //    Console.WriteLine("Aaaaaaaa");
        //    if (this.ProgressBar.Maximum > 0)
        //    {
        //        this.MediaPlayer.Position = TimeSpan.FromSeconds(this.ProgressBar.Value);
        //    }
        //}
    }

    public class FileEventArg : EventArgs
    {
        public ePlayState State { get; private set; }

        public FileEventArg(ePlayState state)
        {
            this.State = state;
        }
    }
}
