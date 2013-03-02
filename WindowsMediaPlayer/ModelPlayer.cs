using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;

namespace WindowsMediaPlayer
{
    class ModelPlayer : INotifyPropertyChanged
    {
        private RessourceManager ressourceManager;
        private MediaHandler mediaHandler;

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand FindRessource { get; private set; }
        public ICommand PlayFile { get; private set; }
        public ICommand StopFile { get; private set; }

        private string lectureContent;
        public string LectureContent
        {
            get
            {
                return this.lectureContent;
            }
            set
            {
                this.lectureContent = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("LectureContent"));
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

            this.LectureContent = "Lecture";
        }
    }
}
