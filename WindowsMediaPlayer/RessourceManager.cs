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

        public RessourceManager()
        {
            this.FileFound = false;
            this.FilePath = null;
        }

        public void FindRessource()
        {
            OpenFileDialog windowsDial = new OpenFileDialog();
            windowsDial.FileName = "File";
            windowsDial.DefaultExt = ".avi";
            windowsDial.Filter = "Music file (.mp3)|*.mp3|" + "Video File (.avi, .wmv)|*.avi;*.wmv|" + "Picture File (.bmp, .jpg, .jpeg, .png)|*.bmp;*.jpg;*.jpeg;*.png";

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
