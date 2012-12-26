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

    //NEED TO FIGURE OUT WHAT ELEMENTS/ATTRIBUTES ARE ALWAYS IN FILE
    //AND WHICH SHOULD BE REMOVED WHEN SET TO NULL
    //FIGURE THIS OUT BY SEEING WHAT DISAPEARS WHEN VALUES ARE UNCHECKED IN TRAKTOR TRACK DETAILS EDITOR

    public class TraktorTrack : System.ComponentModel.INotifyPropertyChanged
    {
        private System.Xml.XmlElement entryNode;

        public TraktorTrack(System.Xml.XmlElement entryNode)
        {
            this.entryNode = entryNode;
        }

        #region Attribute Accessor Methods

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

            if (att == null)
            {
                return null;
            }
            else
            {
                return att.Value;
            }
        }


        private void RemoveAttribute(string elementName, string attributeName)
        {
            System.Xml.XmlNode node;

            if (String.IsNullOrEmpty(elementName))
            {
                node = entryNode;
            }
            else
            {
                node = entryNode.SelectSingleNode("./" + elementName);

                if (node == null)
                {
                    return;
                }
            }
            var att = node.Attributes[attributeName];

            if (att != null)
            {
                node.Attributes.Remove(att);
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
                node = entryNode.SelectSingleNode("./" + elementName);

                if (node == null)
                {
                    node = entryNode.OwnerDocument.CreateElement(elementName);
                    entryNode.AppendChild(node);
                }
            }

            var att = node.Attributes[attributeName];

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

        #endregion



        public TrackDataSourceTag DataSourceTag
        {
            get
            {
                var dataSourceElement = entryNode.SelectSingleNode("./DataSource");

                if (dataSourceElement == null) return null;

                var provider = GetAttributeValue("DataSource", "Name");
                var url = GetAttributeValue("DataSource", "TrackURL");
                var TrackId = GetAttributeValue("DataSource", "TrackId");

                TrackDataSourceTag tag = new TrackDataSourceTag(provider, TrackId, new Uri(url));

                return tag;

            }

            set
            {
                if (value != null)
                {
                    SetAttributeValue("DataSource", "Name", value.DataSource);
                    SetAttributeValue("DataSource", "TrackURL", value.Uri.AbsoluteUri);
                    SetAttributeValue("DataSource", "TrackId", value.TrackId);
                }
                else
                {
                    var dataSourceElement = entryNode.SelectSingleNode("./DataSource");

                    if (dataSourceElement != null)
                    {
                        dataSourceElement.ParentNode.RemoveChild(dataSourceElement);
                    }
                }

                OnPropertyChanged("DataSourceTag");

            }
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
                //Title can not be empty

                if (!String.IsNullOrEmpty(value)) SetAttributeValue(null, "TITLE", value);
                OnPropertyChanged("Title");
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
                OnPropertyChanged("Artist");
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
                OnPropertyChanged("Mix");
            }
        }


        public string Producer
        {
            get
            {
                return GetAttributeValue("INFO", "PRODUCER");
            }
            set
            {

                SetAttributeValue("INFO", "PRODUCER", value);
                OnPropertyChanged("Producer");
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
                OnPropertyChanged("Remixer");
            }
        }

        public string Release
        {
            get
            {
                return GetAttributeValue("ALBUM", "TITLE");
            }
            set
            {

                SetAttributeValue("ALBUM", "TITLE", value);
                OnPropertyChanged("Release");
            }
        }

        public Key Key
        {
            get
            {
                try
                {
                    string keyString = GetAttributeValue("INFO", "KEY");

                    if (String.IsNullOrEmpty(keyString)) return null;
                    else return new Key(keyString);
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                if (value != null)
                {
                    SetAttributeValue("INFO", "KEY", value.ToString());
                }
                else
                {
                    RemoveAttribute("INFO", "KEY");
                }

                OnPropertyChanged("Key");
            }
        }



        public double BPM
        {
            get
            {
                return System.Convert.ToDouble(GetAttributeValue("TEMPO", "BPM"));
            }
            set
            {

                SetAttributeValue("TEMPO", "BPM", value.ToString());
                OnPropertyChanged("BPM");
            }
        }

        public int? BPMQuality
        {
            get
            {
                return System.Convert.ToInt32(GetAttributeValue("TEMPO", "BPM_QUALITY"));
            }
            set
            {

                SetAttributeValue("BPM_QUALITY", "BPM", value.ToString());
                OnPropertyChanged("BPMQuality");
            }
        }


        public string Lyrics
        {
            get
            {
                return GetAttributeValue("INFO", "KEY_LYRICS");
            }
            set
            {

                SetAttributeValue("INFO", "KEY_LYRICS", value);
                OnPropertyChanged("Lyrics");
            }
        }

        public string Label
        {
            get
            {
                return GetAttributeValue("INFO", "LABEL");
            }
            set
            {

                SetAttributeValue("INFO", "LABEL", value);
                OnPropertyChanged("Label");
            }
        }

        public string CatalogNumber
        {
            get
            {
                return GetAttributeValue("INFO", "CATALOG_NO");
            }
            set
            {

                SetAttributeValue("INFO", "CATALOG_NO", value);
                OnPropertyChanged("CatalogNumber");
            }
        }

        public string Genre
        {
            get
            {
                return GetAttributeValue("INFO", "GENRE");
            }
            set
            {

                SetAttributeValue("INFO", "GENRE", value);
                OnPropertyChanged("Genre");
            }
        }

        public string Comment1
        {
            get
            {
                return GetAttributeValue("INFO", "COMMENT");
            }
            set
            {

                SetAttributeValue("INFO", "COMMENT", value);
                OnPropertyChanged("Comment1");
            }
        }

        public string Comment2
        {
            get
            {
                return GetAttributeValue("INFO", "RATING");
            }
            set
            {

                SetAttributeValue("INFO", "RATING", value);
                OnPropertyChanged("Comment2");
            }
        }


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
                throw new NotImplementedException();



            }

        }
        public DateTime ImportDate
        {
            get
            {
                return DateTime.Parse(GetAttributeValue("INFO", "IMPORT_DATE"));
            }
        }


        public long FileSize
        {
            get
            {
                string sizeStr = GetAttributeValue("INFO", "FILESIZE");
                if (sizeStr == null) return 0;
                return System.Convert.ToInt64(sizeStr);
            }
        }
        public int BitRate
        {
            get
            {
                string bitRateStr = GetAttributeValue("INFO", "BITRATE");
                if (bitRateStr == null) return 0;
                return System.Convert.ToInt32(bitRateStr);
            }
        }
        public Location Location
        {
            get
            {
                return null;
            }
        }




        public Rating? Rating
        {
            get
            {

                //determine star value from Rating value


                if (RatingValue.HasValue)
                {
                    Decimal value = Math.Round((decimal)RatingValue / (decimal)255 * (decimal)5);

                    int intValue = System.Convert.ToInt32(value);

                    return (Rating)intValue;

                }
                else
                {
                    return null;
                }



            }


            set
            {

                RatingValue = ((int)value) * 51;
                OnPropertyChanged("Rating");
            }


        }




        public int? RatingValue
        {
            get
            {
                var str = GetAttributeValue("INFO", "RANKING");

                if (str == null) return null;

                return System.Convert.ToInt32(str);
            }
            set
            {
                if (value.HasValue)
                {
                    SetAttributeValue("INFO", "RANKING", value.ToString());
                }
                else
                {
                    SetAttributeValue("INFO", "RANKING", null);
                }


            }
        }

        public DateTime? ReleaseDate
        {
            get
            {

                var str = GetAttributeValue("INFO", "RELEASE_DATE");

                if (String.IsNullOrEmpty(str)) return null;

                var returndate = DateTime.Parse(str);

                return returndate;
            }
            set
            {
                if (!value.HasValue)
                {
                    SetAttributeValue("INFO", "RELEASE_DATE", null);
                }
                else
                {
                    string valueString = value.Value.ToString("yyyy/M/d");

                    SetAttributeValue("INFO", "RELEASE_DATE", valueString);
                }


                OnPropertyChanged("ReleaseDate");

            }
        }


        public int? PlayTime
        {
            get
            {
                var str = GetAttributeValue("INFO", "PLAYTIME");

                if (String.IsNullOrEmpty(str)) return null;

                return System.Convert.ToInt32(str);
            }
        }
        public double? PlayTimeFloat
        {
            get
            {
                var str = GetAttributeValue("INFO", "PLAYTIME_FLOAT");

                if (String.IsNullOrEmpty(str)) return null;

                return System.Convert.ToDouble(str);
            }
        }

        public int? PlayCount
        {
            get
            {
                var str = GetAttributeValue("INFO", "PLAYCOUNT");

                if (String.IsNullOrEmpty(str)) return null;

                return System.Convert.ToInt32(str);
            }
        }


        public override string ToString()
        {
            return this.Artist + " " + this.Title + " " + this.Mix + " " + this.Label;
        }


        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            System.ComponentModel.PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(name));
            }
        }

    }
}


