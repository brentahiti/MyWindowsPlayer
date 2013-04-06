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
        public ObservableCollection<PlayListElement> Music { get; set; }
        public ObservableCollection<PlayListElement> Video { get; set; }
        public ObservableCollection<PlayListElement> Picture { get; set; }

        public Library()
        {
            this.Music = new ObservableCollection<PlayListElement>();
            this.Video = new ObservableCollection<PlayListElement>();
            this.Picture = new ObservableCollection<PlayListElement>();
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
