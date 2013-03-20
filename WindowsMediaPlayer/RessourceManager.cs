using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Win32;

namespace WindowsMediaPlayer
{
    public class RessourceManager
    {
        public bool FileFound { get; private set; }
        public string FilePath { get; private set; }

        public int currentElementInPlaylist { get; set; }
        public int numberElementInPlaylist { get; set; }
        public Playlist playlist { get; private set; }
        public bool playlistFound { get; private set; }

        public RessourceManager()
        {
            this.FileFound = false;
            this.FilePath = null;
            this.playlist = new Playlist();
            this.playlistFound = false;
            this.numberElementInPlaylist = 0;
            this.currentElementInPlaylist = 0;
        }

        public void AddElementInPlaylist()
        {
            OpenFileDialog windowsDial = new OpenFileDialog();

            windowsDial.FileName = "File";
            windowsDial.DefaultExt = ".avi";
            windowsDial.Filter = "Music file (.mp3)|*.mp3|" + "Video File (.avi, .wmv)|*.avi;*.wmv";

            Nullable<bool> result = windowsDial.ShowDialog();
            if (result == true)
            {
                PlayListElement tmpElement = new PlayListElement();
                tmpElement.Path = windowsDial.FileName;
                this.playlist.playList.Add(tmpElement);
                this.numberElementInPlaylist = this.playlist.playList.Count();
                playlistFound = true;
                this.FileFound = true;
            }
        }

        public void SavePlayList()
        {
            OpenFileDialog windowsDial = new OpenFileDialog();
            windowsDial.FileName = "PlayList";
            windowsDial.DefaultExt = "xml";
            windowsDial.Filter = "Fichier xml(*.xml)|*.xml";
            Nullable<bool> result = windowsDial.ShowDialog();
            if (result == true)
            {
                this.playlist.saveList(windowsDial.FileName);
            }
        }

        public void LoadPlayList()
        {
            OpenFileDialog windowsDial = new OpenFileDialog();
            windowsDial.FileName = "PlayList";
            windowsDial.DefaultExt = "xml";
            windowsDial.Filter = "Fichier xml(*.xml)|*.xml";
            Nullable<bool> result = windowsDial.ShowDialog();
            if (result == true)
            {
                this.playlist = this.playlist.loadList(windowsDial.FileName);
                this.numberElementInPlaylist = this.playlist.playList.Count();
                this.currentElementInPlaylist = 0;
                playlistFound = true;
                this.FileFound = true;
            }
        }

        public void FindRessource()
        {
            OpenFileDialog windowsDial = new OpenFileDialog();
            windowsDial.FileName = "";
            windowsDial.DefaultExt = "avi";
            windowsDial.Filter = "All files (*.*)|*.*|" + "Music file (*.mp3)|*.mp3|" + "Video File (*.avi, *.wmv)|*.avi;*.wmv|" + "Picture File (*.bmp, *.jpg, *.jpeg, *.png)|*.bmp;*.jpg;*.jpeg;*.png";

            Nullable<bool> result = windowsDial.ShowDialog();
            if (result == true)
            {
                this.FileFound = true;
                this.FilePath = windowsDial.FileName;
            }
            else
            {
                this.FileFound = false;
            }
        }
    }
}
