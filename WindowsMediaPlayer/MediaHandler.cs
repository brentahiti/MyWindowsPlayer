using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Threading;

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

        public MediaHandler(RessourceManager rm)
        {
            this.PlayState = ePlayState.Stop;

            this.MediaPlayer = new MediaElement();
            this.MediaPlayer.VerticalAlignment = VerticalAlignment.Center;
            this.MediaPlayer.Height = Double.NaN;
            this.MediaPlayer.Width = Double.NaN;
            this.MediaPlayer.LoadedBehavior = MediaState.Manual;
            this.MediaPlayer.MediaOpened += new RoutedEventHandler(OnMediaOpened);

            this.ProgressBar = new Slider();
            this.ProgressBar.Value = 0.0;
            this.ProgressBar.Maximum = 1.0;
            this.ProgressBar.MouseLeftButtonUp += new MouseButtonEventHandler(OnClickProgressBar);

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
            }
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

        private void MediaTimerTick(object sender, EventArgs e)
        {
            if (this.ProgressBar.Maximum > 0)
            {
                this.ProgressBar.Value = this.MediaPlayer.Position.TotalSeconds;
            }
        }

        private void OnClickProgressBar(object sender, MouseButtonEventArgs e)
        {
            if (this.ProgressBar.Maximum > 0)
            {
                this.MediaPlayer.Position = TimeSpan.FromSeconds(this.ProgressBar.Value);
            }
        }
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
