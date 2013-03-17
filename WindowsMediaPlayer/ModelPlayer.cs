using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Controls;

namespace WindowsMediaPlayer
{
    public class ModelPlayer : INotifyPropertyChanged
    {
        private RessourceManager ressourceManager;
        private MediaHandler mediaHandler;
        public MediaElement mediaElement { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand FindRessource { get; private set; }
        public ICommand PlayFile { get; private set; }
        public ICommand StopFile { get; private set; }

        private string visiblePlay;
        private string visiblePause;
        public string VisiblePlay
        {
            get { return this.visiblePlay; }
            private set
            {
                this.visiblePlay = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("VisiblePlay"));
            }
        }
        public string VisiblePause // { get; private set; }
        {
            get { return this.visiblePause; }
            private set
            {
                this.visiblePause = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("VisiblePause"));
            }
        }

        private double valueSoundContent;
        public double ValueSoundContent
        {
            get { return this.valueSoundContent; }
            set
            {
                this.valueSoundContent = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("ValueSoundContent"));
                    this.mediaElement.Volume = this.valueSoundContent / 100;
                }
            }
        }

        public ModelPlayer()
        {
            this.ressourceManager = new RessourceManager();
            this.mediaHandler = new MediaHandler(this.ressourceManager);

            this.FindRessource = new RelayCommand(this.ressourceManager.FindRessource);
            this.PlayFile = new RelayCommand(this.mediaHandler.PlayFile);
            this.StopFile = new RelayCommand(this.mediaHandler.StopFile);
            this.mediaElement = this.mediaHandler.MediaPlayer;

            this.mediaHandler.FileEvent += new EventHandler<FileEventArg>(ChangeLectureContent);

            this.ValueSoundContent = 50.0;
            this.VisiblePlay = "Visible";
            this.VisiblePause = "Hidden";
        }

        public void ChangeLectureContent(object sender, FileEventArg e)
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
    }
}
