using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace WindowsMediaPlayer
{
    public enum ePlayState { Play, Pause, Stop }

    class FilePlayer : ICommand
    {
        private MediaHandler mediaHandler;

        public FilePlayer(MediaHandler mh)
        {
            this.mediaHandler = mh;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (this.mediaHandler.RessourceManager.FindRessource.FileFound)
            {
                if (this.mediaHandler.PlayState == ePlayState.Play)
                {
                    this.mediaHandler.MediaPlayer.Pause();
                    this.mediaHandler.PlayState = ePlayState.Pause;
                }
                else
                {
                    if (this.mediaHandler.PlayState == ePlayState.Stop)
                    {
                        this.mediaHandler.MediaPlayer.Open(new System.Uri(this.mediaHandler.RessourceManager.FindRessource.FilePath));
                    }
                    this.mediaHandler.MediaPlayer.Play();
                    this.mediaHandler.PlayState = ePlayState.Play;
                }
            }
        }
    }

    class FileStopper : ICommand
    {
        private MediaHandler mediaHandler;

        public FileStopper(MediaHandler mh)
        {
            this.mediaHandler = mh;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (this.mediaHandler.PlayState != ePlayState.Stop)
            {
                this.mediaHandler.MediaPlayer.Stop();
                this.mediaHandler.PlayState = ePlayState.Stop;
            }
        }
    }

    class MediaHandler
    {
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

        public MediaPlayer MediaPlayer { get; private set; }
        public RessourceManager RessourceManager { get; private set; }
        public FilePlayer PlayFile { get; private set; }
        public FileStopper StopFile { get; private set; }

        public event EventHandler<FileEventArg> FileEvent;

        public MediaHandler(RessourceManager rm)
        {
            this.PlayState = ePlayState.Stop;
            this.MediaPlayer = new MediaPlayer();
            this.RessourceManager = rm;
            this.PlayFile = new FilePlayer(this);
            this.StopFile = new FileStopper(this);
        }
    }

    class FileEventArg : EventArgs
    {
        public ePlayState State { get; private set; }

        public FileEventArg(ePlayState state)
        {
            this.State = state;
        }
    }
}
