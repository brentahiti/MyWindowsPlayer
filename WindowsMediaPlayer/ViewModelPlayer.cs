using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;

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
        public Slider ProgressBar
        {
            get { return this.mediaHandler.ProgressBar; }
        }


        private RessourceManager ressourceManager;
        private MediaHandler mediaHandler;

        public ICommand FindRessource { get; private set; }
        public ICommand PlayFile { get; private set; }
        public ICommand StopFile { get; private set; }
        public ICommand SavePlayList { get; private set; }
        public ICommand LoadPlayList { get; private set; }
        public ICommand AddElementInPlaylist { get; private set; }
        
        public ViewModelPlayer()
        {
            this.ressourceManager = new RessourceManager();
            this.mediaHandler = new MediaHandler(this.ressourceManager);

            this.FindRessource = new RelayCommand(this.ressourceManager.FindRessource);
            this.PlayFile = new RelayCommand(this.mediaHandler.PlayFile);
            this.StopFile = new RelayCommand(this.mediaHandler.StopFile);
            this.SavePlayList = new RelayCommand(this.ressourceManager.SavePlayList);
            this.LoadPlayList = new RelayCommand(this.ressourceManager.LoadPlayList);
            this.AddElementInPlaylist = new RelayCommand(this.ressourceManager.AddElementInPlaylist);

            this.mediaHandler.FileEvent += new EventHandler<FileEventArg>(ChangeLectureContent);
            this.mediaHandler.FileLoaded += new EventHandler(OnFileLoaded);
            this.mediaHandler.FileEnded += new EventHandler(OnFileEnded);

            this.ValueSoundContent = 50.0;
            this.VisiblePlay = "Visible";
            this.VisiblePause = "Hidden";
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

        private void OnFileLoaded(object sender, EventArgs e)
        {
            this.ProgressBarMaxSpan = this.mediaHandler.FileDuration;
        }

        private void OnFileEnded(object sender, EventArgs e)
        {
            if (this.ressourceManager.playlistFound == true)
            {
                this.mediaHandler.PlayState = ePlayState.Stop;
                this.mediaHandler.PlayFile();
            }
        }
    }
}
