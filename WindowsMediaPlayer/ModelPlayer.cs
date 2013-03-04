using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Controls;

namespace WindowsMediaPlayer
{
    class ModelPlayer : INotifyPropertyChanged
    {
        private RessourceManager ressourceManager;
        private MediaHandler mediaHandler;
        public MediaElement mediaElement { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand FindRessource { get; private set; }
        public ICommand PlayFile { get; private set; }
        public ICommand StopFile { get; private set; }

        private string lectureContent;
        public string LectureContent
        {
            get { return this.lectureContent; }
            set 
            {
                this.lectureContent = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("LectureContent"));
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

            this.FindRessource = this.ressourceManager.FindRessource;
            this.PlayFile = this.mediaHandler.PlayFile;
            this.StopFile = this.mediaHandler.StopFile;
            this.mediaElement = this.mediaHandler.MediaPlayer;

            this.mediaHandler.FileEvent += new EventHandler<FileEventArg>(ChangeLectureContent);

            this.LectureContent = "Play";
            this.ValueSoundContent = 50.0;
        }

        public void ChangeLectureContent(object sender, FileEventArg e)
        {
            if (e.State == ePlayState.Play)
                this.LectureContent = "Pause";
            else
                this.LectureContent = "Play";
        }
    }
}
