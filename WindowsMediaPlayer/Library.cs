using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace WindowsMediaPlayer
{
    public class Library
    {
        public ObservableCollection<PlayListElement> music;
        public ObservableCollection<PlayListElement> Music
        {
            get { return music; }
            set { music = value; }
        }

        public ObservableCollection<PlayListElement> video;
        public ObservableCollection<PlayListElement> Video
        {
            get { return video; }
            set { video = value; }
        }

        public ObservableCollection<PlayListElement> picture;
        public ObservableCollection<PlayListElement> Picture
        {
            get { return picture; }
            set { picture = value; }
        }

        public Library()
        {
            this.music = new ObservableCollection<PlayListElement>();
            this.video = new ObservableCollection<PlayListElement>();
            this.picture = new ObservableCollection<PlayListElement>();
        }

        public Library load(string name)
        {
            try
            {
                FileStream file = File.Open(name, FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(Library));
                Library newList = (Library)serializer.Deserialize(file);
                file.Close();
                return newList;
            }
            catch (FileNotFoundException e)
            {
                return this;
            }
        }


        public void save(string name)
        {
            FileStream file = File.Open(name, FileMode.OpenOrCreate);
            XmlSerializer serializer = new XmlSerializer(typeof(Library));
            serializer.Serialize(file, this);
            file.Close();
        }
    }
}
