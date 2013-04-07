using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;

namespace WindowsMediaPlayer
{
    public enum PathOfMedia { PLAYLIST_MEDIA, LIBRARY_MEDIA, OTHER_MEDIA};

    public class RessourceManager
    {
        public bool FileFound { get; set; }
        public string FilePath { get; private set; }

        public PlayListElement SelectedItem { get; set; }
        public PlayListElement SelectedPicture { get; set; }
        public PlayListElement SelectedMusic { get; set; }
        public PlayListElement SelectedVideo { get; set; }
        public int CurrentElementInPlaylist { get; set; }
        public int NumberElementInPlaylist { get; set; }
        public Playlist Playlist { get; private set; }
        public bool PlaylistFound { get; private set; }
        public Library Library { get; private set; }

        public PathOfMedia TypeOfMedia {get; set;}

        public RessourceManager()
        {
            this.FileFound = false;
            this.FilePath = null;
            this.Playlist = new Playlist();
            this.Library = new Library();
            this.Library = this.Library.load("C:\\Users\\" + Environment.UserName + "\\Documents\\LibraryMediaPLayer.xml");
            this.PlaylistFound = false;
            this.NumberElementInPlaylist = 0;
            this.CurrentElementInPlaylist = 0;
        }

        public void AddToLibrary()
        {
            OpenFileDialog windowsDial = new OpenFileDialog();

            windowsDial.FileName = "File";
            windowsDial.DefaultExt = ".avi";
            windowsDial.Filter = "Music file (.mp3)|*.mp3|" + "Video File (.avi, .wmv)|*.avi;*.wmv|" + "Picture File (*.bmp, *.jpg, *.jpeg, *.png)|*.bmp;*.jpg;*.jpeg;*.png";

            Nullable<bool> result = windowsDial.ShowDialog();
            if (result == true)
            {
                PlayListElement tmpElement = new PlayListElement();
                tmpElement.Pathname = windowsDial.FileName;
                string ext = Path.GetExtension(tmpElement.Pathname);
                if (ext == ".mp3")
                    this.Library.Music.Add(tmpElement);
                else if (ext == ".avi" || ext == ".wmv")
                    this.Library.Video.Add(tmpElement);
                else if (ext == ".bmp" || ext == ".jpg" || ext == ".jpeg" || ext == ".png")
                    this.Library.Picture.Add(tmpElement);
                {
                    this.Library.save("C:\\Users\\" + Environment.UserName + "\\Documents\\LibraryMediaPLayer.xml");
                }
            }
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
                tmpElement.Pathname = windowsDial.FileName;
                this.Playlist.Elements.Add(tmpElement);
                this.NumberElementInPlaylist = this.Playlist.Elements.Count();
                PlaylistFound = true;
                this.FileFound = true;
            }
        }

        public void SavePlayList()
        {
            SaveFileDialog windowsDial = new SaveFileDialog();
            windowsDial.FileName = "PlayList";
            windowsDial.DefaultExt = "xml";
            windowsDial.Filter = "Fichier xml(*.xml)|*.xml";
            Nullable<bool> result = windowsDial.ShowDialog();
            if (result == true)
            {
                this.Playlist.saveList(windowsDial.FileName);
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
                this.Playlist = this.Playlist.loadList(windowsDial.FileName);
                this.NumberElementInPlaylist = this.Playlist.Elements.Count();
                this.CurrentElementInPlaylist = 0;
                PlaylistFound = true;
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
                this.CurrentElementInPlaylist = 0;
                this.PlaylistFound = false;
                this.TypeOfMedia = PathOfMedia.OTHER_MEDIA;
                this.NumberElementInPlaylist = 0;
                this.Playlist.Elements.Clear();
                this.FilePath = windowsDial.FileName;
            }
            else
            {
                this.FileFound = false;
            }
        }
    }
}
