using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace WindowsMediaPlayer
{
    public class PlayListElement
    {
        public string path;
        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        public PlayListElement()
        {
        }
    }
}
