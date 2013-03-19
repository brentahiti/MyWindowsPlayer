using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Threading;

namespace WindowsMediaPlayer
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class ViewModelPlayer : ViewModelBase
    {
        private string visiblePlay;
        public string VisiblePlay
        {
            get { return this.visiblePlay; }
            private set
            {
                this.visiblePlay = value;
                NotifyPropertyChanged("VisiblePlay");
            }
        }
        private string visiblePause;
        public string VisiblePause // { get; private set; }
        {
            get { return this.visiblePause; }
            private set
            {
                this.visiblePause = value;
                NotifyPropertyChanged("VisiblePause");
            }
        }

        private double valueSoundContent;
        public double ValueSoundContent
        {
            get { return this.valueSoundContent; }
            set
            {
                this.valueSoundContent = value;
                this.mediaHandler.MediaPlayer.Volume = this.valueSoundContent / 100.0;
                NotifyPropertyChanged("ValueSoundContent");
            }
        }
        private double progressBarMax;
        public double ProgressBarMax
        {
            get { return this.progressBarMax; }
            set
            {
                this.progressBarMax = value;
                NotifyPropertyChanged("ProgressBarMax");
            }
        }
        private double progressBarVal;
        public double ProgressBarVal
        {
            get { return this.progressBarVal; }
            set
            {
                this.progressBarVal = value;
                NotifyPropertyChanged("ProgressBarVal");
            }
        }
        private TimeSpan progressBarMaxSpan;
        public TimeSpan ProgressBarMaxSpan
        {
            get { return this.progressBarMaxSpan; }
            set
            {
                this.progressBarMaxSpan = value;
                NotifyPropertyChanged("ProgressBarMaxSpan");
            }
        }
        public MediaElement MyMediaPlayer
        {
            get { return this.mediaHandler.MediaPlayer; }
        }


        private RessourceManager ressourceManager;
        private MediaHandler mediaHandler;
        private DispatcherTimer mediaTimer;

        public ICommand FindRessource { get; private set; }
        public ICommand PlayFile { get; private set; }
        public ICommand StopFile { get; private set; }
        
        public ViewModelPlayer()
        {
            this.ressourceManager = new RessourceManager();
            this.mediaHandler = new MediaHandler(this.ressourceManager);

            this.FindRessource = new RelayCommand(this.ressourceManager.FindRessource);
            this.PlayFile = new RelayCommand(this.mediaHandler.PlayFile);
            this.StopFile = new RelayCommand(this.mediaHandler.StopFile);

            this.mediaHandler.FileEvent += new EventHandler<FileEventArg>(ChangeLectureContent);
            this.mediaHandler.MediaPlayer.MediaOpened += new RoutedEventHandler(OnMediaOpened);

            this.ValueSoundContent = 50.0;
            this.VisiblePlay = "Visible";
            this.VisiblePause = "Hidden";
            this.ProgressBarVal = 0.0;
            this.ProgressBarMax = 1.0;
        }

        private void ChangeLectureContent(object sender, FileEventArg e)
        {
            Console.WriteLine("ok");
            if (e.State == ePlayState.Play)
            {
                this.VisiblePlay = "Hidden";
                this.VisiblePause = "Visible";
            }
            else
            {
                this.VisiblePlay = "Visible";
                this.VisiblePause = "Hidden";
            }
        }

        private void OnMediaOpened(object sender, RoutedEventArgs e)
        {
            this.ProgressBarMaxSpan = this.mediaHandler.MediaPlayer.NaturalDuration.TimeSpan;
            this.ProgressBarMax = this.ProgressBarMaxSpan.TotalSeconds;

            this.mediaTimer = new DispatcherTimer();
            this.mediaTimer.Interval = TimeSpan.FromSeconds(1);
            this.mediaTimer.Tick += new EventHandler(MediaTimerTick);
            this.mediaTimer.Start();
        }

        private void MediaTimerTick(object sender, EventArgs e)
        {
            if (this.ProgressBarMax > 0)
            {
                this.ProgressBarVal = this.mediaHandler.MediaPlayer.Position.TotalSeconds;
            }
        }
    }
}
