using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace WindowsMediaPlayer
{
    public class Playlist
    {
        public List<PlayListElement> playList;
        public List<PlayListElement> PlayList
        {
            get { return playList; }
            set { playList = value; }
        }

        public void saveList(string name)
        {
            FileStream file = File.Open(name, FileMode.OpenOrCreate);
            XmlSerializer serializer = new XmlSerializer(typeof(Playlist));
            serializer.Serialize(file, this);
            file.Close();
        }

        public Playlist loadList(string name)
        {
            FileStream file = File.Open(name, FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(Playlist));
            Playlist newList = (Playlist)serializer.Deserialize(file);
            file.Close();
            return newList;
        }

        public Playlist()
        {
            playList = new List<PlayListElement>();
        }
    }
}
