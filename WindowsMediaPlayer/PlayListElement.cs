using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;


namespace WindowsMediaPlayer
{
    public class PlayListElement
    {
        private string path;
        public string Pathname
        {
            get { return this.path; }
            set
            {
                this.path = value;
                this.Filename = Path.GetFileName(this.path);
            }
        }

        public string Filename { get; set; }

        //public override bool Equals(object o)
        //{
        //    if (o == null)
        //        return false;

        //    return ((PlayListElement)o).Pathname == this.Pathname;
        //}

        public override int GetHashCode()
        {
            return this.Pathname.GetHashCode();
        }

        public override string ToString()
        {
            return this.Filename;
        }

        public PlayListElement()
        {
        }
    }
}
