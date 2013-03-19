using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace WindowsMediaPlayer
{
    public enum ePlayState { Play, Pause, Stop }

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
        public MediaElement MediaPlayer { get; set; }
        public RessourceManager RessourceManager { get; private set; }

        public event EventHandler<FileEventArg> FileEvent;

        public MediaHandler(RessourceManager rm)
        {
            this.PlayState = ePlayState.Stop;
            this.MediaPlayer = new MediaElement();
            this.MediaPlayer.VerticalAlignment = VerticalAlignment.Center;
            this.MediaPlayer.Height = Double.NaN;
            this.MediaPlayer.Width = Double.NaN;
            this.MediaPlayer.LoadedBehavior = MediaState.Manual;

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
