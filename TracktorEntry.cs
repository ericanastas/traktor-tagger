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
                return GetAttributeValue(null, "TITLE");
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) SetAttributeValue(null, "TITLE", value);
            }
        }


        private string GetAttributeValue(string elementName, string attributeName)
        {
            System.Xml.XmlNode node;

            if (String.IsNullOrEmpty(elementName))
            {
                node = entryNode;
            }
            else
            {
                node = entryNode.SelectSingleNode("./" + elementName);
            }

            if (node == null) return null;

            var att = node.Attributes[attributeName];

            if(att == null)
            {
                return null;
            }
            else
            {
                return att.Value;
            }        
        }

        private void SetAttributeValue(string elementName, string attributeName, string value)
        {

            System.Xml.XmlNode node;

            if (String.IsNullOrEmpty(elementName))
            {
                node = entryNode;
            }
            else
            {
                node = entryNode.FirstChild[elementName];

                if (node == null)
                {
                    node = entryNode.OwnerDocument.CreateElement(elementName);
                    entryNode.AppendChild(node);
                }
            }

            var att = entryNode.SelectSingleNode(elementName).Attributes[attributeName];

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
                return GetAttributeValue(null, "ARTIST");
            }
            set
            {
                SetAttributeValue(null, "ARTIST", value);      
            }
        }


        public string Mix
        {
            get
            {
                return GetAttributeValue("INFO", "MIX");
            }
            set
            {
                SetAttributeValue("INFO", "MIX", value);
            }
        }


        public string Remixer 
        {
            get
            {
                return GetAttributeValue("INFO", "REMIXER");
            }
            set
            {
                SetAttributeValue("INFO", "REMIXER", value);
            }
        }

        public string AlbumTitle
        {
            get
            {
                return GetAttributeValue("ALBUM", "TITLE");
            }
            set
            {
                SetAttributeValue("ALBUM", "TITLE", value);
            }
        }

        public string Key { get; set; }
        public double BPM { get; set; }
        public string Lyrics { get; set; }

        public string Label { get; set; }
        public string CatalogNumber { get; set; }

        public string Genre { get; set; }

        public string Comment1 { get; set; }
        public string Comment2 { get; set; }


        public DateTime ModifiedDate 
        {

            get
            {
                string dateStr = GetAttributeValue(null, "MODIFIED_DATE");
                long ticks = System.Convert.ToInt64(GetAttributeValue(null, "MODIFIED_TIME"));

                DateTime date = DateTime.Parse(dateStr);
                date = date.AddSeconds(ticks);

                return date;
            }
            set
            { 
            
            
            
            }
        
        }
        public DateTime ImportDate { get; set; }


        public int FileSize {
            get
            {
                return 0;
            }
        }
        public int BitRate
        {
            get
            {
                return 0;
            }
        }
        public Location Location
        {
            get
            {
                return null;
            }
        }        

        public int Ranking { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int PlayTime
        {
            get
            {
                return 0;
            }
        }
        public double PlayTimeFloat
        {
            get
            {
                return 0;
            }
        }


        public override string ToString()
        {
            return this.Artist + " " + this.Title + " " + this.Mix + " " + this.Label;
        }



    }
}


