using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace WindowsMediaPlayer
{
    public enum TypeMedia { NONE, MUSIC, VIDEO, PICTURE };

    public class Library
    {
        public ObservableCollection<PlayListElement> Music { get; set; }
        public ObservableCollection<PlayListElement> Video { get; set; }
        public ObservableCollection<PlayListElement> Picture { get; set; }

        public TypeMedia actualList { get; set; }
        public Int32 musicList { get; set; }
        public Int32 videoList { get; set; }
        public Int32 pictureList { get; set; }

        public Library()
        {
            this.Music = new ObservableCollection<PlayListElement>();
            this.Video = new ObservableCollection<PlayListElement>();
            this.Picture = new ObservableCollection<PlayListElement>();
            this.actualList = TypeMedia.NONE;
        }

        public Library load(string name)
        {
            try
            {
                FileStream file = File.Open(name, FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(Library));
                Library newList = (Library)serializer.Deserialize(file);
                this.musicList = newList.Music.Count();
                this.videoList = newList.Video.Count();
                this.pictureList = newList.Picture.Count();
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
            this.musicList = this.Music.Count();
            this.videoList = this.Video.Count();
            this.pictureList = this.Picture.Count();
            file.Close();
        }
    }
}
