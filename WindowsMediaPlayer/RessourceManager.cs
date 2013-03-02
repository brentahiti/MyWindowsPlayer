using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Win32;

namespace WindowsMediaPlayer
{
    class RessourceFinder : ICommand
    {
        public bool FileFound { get; private set; }
        public string FilePath { get; private set; }

        public RessourceFinder()
        {
            this.FileFound = false;
            this.FilePath = null;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            OpenFileDialog windowsDial = new OpenFileDialog();
            windowsDial.FileName = "File";
            windowsDial.DefaultExt = ".avi";
            windowsDial.Filter = "Music file (.mp3)|*.mp3| " + "Video File (.avi)|*.avi";

            Nullable<bool> result = windowsDial.ShowDialog();
            if (result == true)
            {
                this.FileFound = true;
                this.FilePath = windowsDial.FileName;
            }
            this.FileFound = false;
        }
    }

    class RessourceManager
    {
        public RessourceFinder FindRessource { get; private set; }

        public RessourceManager()
        {
            this.FindRessource = new RessourceFinder();
        }
    }
}
