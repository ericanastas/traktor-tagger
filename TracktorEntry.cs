using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracktorTagger
{

    public class Location
    {

        public string Directory { get; set; }
        public string FileName { get; set; }
        public string Volume { get; set; }
        public string VolumeId { get; set; }

    }

    public class TracktorEntry
    {
        private System.Xml.XmlElement entryNode;

        public TracktorEntry(System.Xml.XmlElement entryNode)
        {
            this.entryNode = entryNode;
        }


        public string Title 
        {
            //Title Attribute is always there;
            get
            {
                return GetAttributeValue(".", "TITLE");
            }
            set
            {
                SetAttributeValue(".", "TITLE", value);
            }
        }


        private string GetAttributeValue(string xpath, string attributeName)
        {
            var node = entryNode.SelectSingleNode(xpath);
            var att = entryNode.SelectSingleNode(xpath).Attributes[attributeName];

            if(att == null)
            {
                return null;
            }
            else
            {
                return att.Value;
            }        
        }


        private void SetAttributeValue(string xpath, string attributeName, string value)
        {
            var node = entryNode.SelectSingleNode(xpath);
            var att = entryNode.SelectSingleNode(xpath).Attributes[attributeName];

            if (att == null)
            {
                var newAtt = node.OwnerDocument.CreateAttribute(attributeName);
                newAtt.Value = value;
                node.Attributes.Append(newAtt);
            }
            else
            {
               att.Value = value;
            }  

        }


        public string Artist
        {
            get
            {
                return GetAttributeValue(".", "ARTIST");
            }
            set
            {
                entryNode.Attributes["ARTIST"].Value = value;
            }
        }


        public string Mix { get; set; }
        public string Remixer { get; set; }
        public string AlbumTitle { get; set; }

        public string Key { get; set; }
        public double BPM { get; set; }
        public string Lyrics { get; set; }

        public string Label { get; set; }
        public string CatalogNumber { get; set; }

        public string Genre { get; set; }

        public string Comment1 { get; set; }
        public string Comment2 { get; set; }


        public DateTime ModifiedDate { get; set; }
        public DateTime ImportDate { get; set; }


        public int FileSize { get; set; }
        public int BitRate { get; set; }
        public Location Location { get; set; }        

        public int Ranking { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int PlayTime { get; set; }
        public double PlayTimeFloat { get; set; }


        public override string ToString()
        {
            return this.Artist + " " + this.Title + " " + this.Mix + " " + this.Label;
        }



    }
}


