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

            if(String.IsNullOrEmpty(elementName))
            {
                node = entryNode;
            }
            else
            {
                node = entryNode.SelectSingleNode("./" + elementName);
            }

            if(node == null) return null;

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


        private void RemoveAttribute(string elementName, string attributeName)
        {
            System.Xml.XmlNode node;

            if(String.IsNullOrEmpty(elementName))
            {
                node = entryNode;
            }
            else
            {
                node = entryNode.SelectSingleNode("./" + elementName);

                if(node == null)
                {
                    return;
                }
            }
            var att = node.Attributes[attributeName];

            if(att != null)
            {
                node.Attributes.Remove(att);
            }

        }



        private void RemoveElement(string elementName)
        {
            if(String.IsNullOrEmpty(elementName))
            {
                throw new ArgumentNullException("elementName");
            }

            System.Xml.XmlNode node = entryNode.SelectSingleNode("./" + elementName);

            if(node == null)
            {
                return;
            }
            else
            {
                entryNode.RemoveChild(node);
            }
        }

        private void SetAttributeValue(string elementName, string attributeName, string value)
        {

            System.Xml.XmlNode node;

            if(String.IsNullOrEmpty(elementName))
            {
                node = entryNode;
            }
            else
            {
                node = entryNode.SelectSingleNode("./" + elementName);

                if(node == null)
                {
                    node = entryNode.OwnerDocument.CreateElement(elementName);
                    entryNode.AppendChild(node);
                }
            }

            var att = node.Attributes[attributeName];

            if(att == null)
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

                if(dataSourceElement == null) return null;

                var provider = GetAttributeValue("DataSource", "Name");
                var url = GetAttributeValue("DataSource", "TrackURL");
                var TrackId = GetAttributeValue("DataSource", "TrackId");

                TrackDataSourceTag tag = new TrackDataSourceTag(provider, TrackId, new Uri(url));

                return tag;
            }

            set
            {
                if(value != null)
                {
                    SetAttributeValue("DataSource", "Name", value.DataSource);
                    SetAttributeValue("DataSource", "TrackURL", value.Uri.AbsoluteUri);
                    SetAttributeValue("DataSource", "TrackId", value.TrackId);
                }
                else
                {
                    RemoveElement("DataSource");
                }

                OnPropertyChanged("DataSourceTag");

            }
        }


        #region String Properties





        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Available when track is first imported</remarks>
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

                if(!String.IsNullOrEmpty(value))
                {
                    SetAttributeValue(null, "TITLE", value);
                    OnPropertyChanged("Title");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Attribute is removed when blank artist is enteredr</remarks>
        public string Artist
        {
            get
            {
                return GetAttributeValue(null, "ARTIST");
            }
            set
            {

                if(string.IsNullOrEmpty(value))
                {
                    RemoveAttribute(null, "ARTIST");
                }
                else
                {
                    SetAttributeValue(null, "ARTIST", value);
                }

                OnPropertyChanged("Artist");
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Mix attribute is removes when set to empty string </remarks>
        public string Mix
        {
            get
            {
                return GetAttributeValue("INFO", "MIX");
            }
            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    RemoveAttribute("INFO", "MIX");
                }
                else
                {
                    SetAttributeValue("INFO", "MIX", value);
                }

                OnPropertyChanged("Mix");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Producer attribtute is removed when set to blank string, but leave info tag.</remarks>
        public string Producer
        {
            get
            {
                return GetAttributeValue("INFO", "PRODUCER");
            }
            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    RemoveAttribute("INFO", "PRODUCER");
                }
                else
                {
                    SetAttributeValue("INFO", "PRODUCER", value);
                }

                OnPropertyChanged("Producer");

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Remixer attribtute is removed when set to blank string, but leave info tag.</remarks>
        public string Remixer
        {
            get
            {
                return GetAttributeValue("INFO", "REMIXER");
            }
            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    RemoveAttribute("INFO", "REMIXER");
                }
                else
                {
                    SetAttributeValue("INFO", "REMIXER", value);
                }

                OnPropertyChanged("Remixer");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Album element is remove when release is removed</remarks>
        public string Release
        {
            get
            {
                return GetAttributeValue("ALBUM", "TITLE");
            }
            set
            {

                if(string.IsNullOrEmpty(value))
                {
                    RemoveElement("ALBUM");
                }
                else
                {
                    SetAttributeValue("ALBUM", "TITLE", value);
                }

                OnPropertyChanged("Release");
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
                if(string.IsNullOrEmpty(value))
                {
                    RemoveAttribute("INFO", "KEY_LYRICS");
                }
                else
                {
                    SetAttributeValue("INFO", "KEY_LYRICS", value);
                }

                OnPropertyChanged("Lyrics");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Label attribute is removes when set to no label </remarks>
        public string Label
        {
            get
            {
                return GetAttributeValue("INFO", "LABEL");
            }
            set
            {

                if(string.IsNullOrEmpty(value))
                {
                    RemoveAttribute("INFO", "LABEL");
                }
                else
                {
                    SetAttributeValue("INFO", "LABEL", value);
                }

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
                if(string.IsNullOrEmpty(value))
                {
                    RemoveAttribute("INFO", "CATALOG_NO");
                }
                else
                {
                    SetAttributeValue("INFO", "CATALOG_NO", value);
                }

                OnPropertyChanged("CatalogNumber");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Genre attribtute is removed when set to blank string, but leave info tag.</remarks>
        public string Genre
        {
            get
            {
                return GetAttributeValue("INFO", "GENRE");
            }
            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    RemoveAttribute("INFO", "GENRE");
                }
                else
                {
                    SetAttributeValue("INFO", "GENRE", value);
                }


                OnPropertyChanged("Genre");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Attribtute is removed when set to blank string, but leave info tag.</remarks>
        public string Comment1
        {
            get
            {
                return GetAttributeValue("INFO", "COMMENT");
            }
            set
            {

                if(string.IsNullOrEmpty(value))
                {
                    RemoveAttribute("INFO", "COMMENT");
                }
                else
                {
                    SetAttributeValue("INFO", "COMMENT", value);
                }

                OnPropertyChanged("Comment1");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Attribtute is removed when set to blank string, but leave info tag.</remarks>
        public string Comment2
        {
            get
            {
                return GetAttributeValue("INFO", "RATING");
            }
            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    RemoveAttribute("INFO", "RATING");
                }
                else
                {
                    SetAttributeValue("INFO", "RATING", value);
                }

                OnPropertyChanged("Comment2");
            }
        }


        #endregion




        public KeyEnum? Key
        {
            get
            {
                string keyString = GetAttributeValue("INFO", "KEY");

                if(string.IsNullOrEmpty(keyString))
                {
                    return null;
                }
                else
                {
                    return KeyEnumStringConverter.ConvertFromString(keyString);
                }
            }
            set
            {
                if(value.HasValue)
                {
                    string newStr = KeyEnumStringConverter.ConvertToString(value.Value);

                    SetAttributeValue("INFO", "KEY", newStr);
                }
                else
                {
                    RemoveAttribute("INFO", "KEY");
                }

                OnPropertyChanged("Key");
            }
        }






        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Available when track is first imported</remarks>
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




        public double? BPM
        {
            get
            {
                var str = GetAttributeValue("TEMPO", "BPM");

                if(str == null) return null;
                else return System.Convert.ToDouble(str);
            }
            set
            {
                if(value.HasValue)
                {
                    SetAttributeValue("TEMPO", "BPM", value.ToString());
                    SetAttributeValue("TEMPO", "BPM_QUALITY", "100");
                }
                else
                {
                    RemoveElement("TEMPO");
                }
            }

        }


        public Rating? Rating
        {
            get
            {

                //determine star value from Rating value

                if(RatingValue.HasValue)
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

                if(str == null) return null;

                return System.Convert.ToInt32(str);
            }
            set
            {
                if(value.HasValue)
                {
                    if(value >= 0 || value <= 255)
                    {
                        SetAttributeValue("INFO", "RANKING", value.ToString());
                        OnPropertyChanged("RatingValue");
                    }
                }
                else
                {
                    RemoveAttribute("INFO", "RANKING");
                    OnPropertyChanged("RatingValue");
                }

            }
        }


        public DateTime? ReleaseDate
        {
            get
            {
                var str = GetAttributeValue("INFO", "RELEASE_DATE");

                if(String.IsNullOrEmpty(str)) return null;

                var returndate = DateTime.Parse(str);

                return returndate;
            }
            set
            {
                if(value.HasValue)
                {
                    string valueString = value.Value.ToString("yyyy/M/d");
                    SetAttributeValue("INFO", "RELEASE_DATE", valueString);
                }
                else
                {
                    RemoveAttribute("INFO", "RELEASE_DATE");
                }


                OnPropertyChanged("ReleaseDate");

            }
        }


        public override string ToString()
        {
            return this.Artist + " " + this.Title + " " + this.Mix + " " + this.Label;
        }


        #region Propertise that need work

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Available when track is first imported</remarks>
        public Location Location
        {
            get
            {
                return null;
            }
            set
            {

                throw new NotImplementedException();
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            System.ComponentModel.PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null)
            {
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(name));
            }
        }

        #endregion


        #region Read Only Properties



        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Available when track is first imported</remarks>
        public DateTime ImportDate
        {
            get
            {
                return DateTime.Parse(GetAttributeValue("INFO", "IMPORT_DATE"));
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Available when track is first imported</remarks>
        public long FileSize
        {
            get
            {
                string sizeStr = GetAttributeValue("INFO", "FILESIZE");
                if(sizeStr == null) return 0;
                return System.Convert.ToInt64(sizeStr);
            }
        }


        public int BitRate
        {
            get
            {
                string bitRateStr = GetAttributeValue("INFO", "BITRATE");
                if(bitRateStr == null) return 0;
                return System.Convert.ToInt32(bitRateStr);
            }
        }


        public int PlayCount
        {
            get
            {
                var str = GetAttributeValue("INFO", "PLAYCOUNT");

                if(String.IsNullOrEmpty(str)) return 0;

                return System.Convert.ToInt32(str);
            }
        }

        public int? PlayTime
        {
            get
            {
                var str = GetAttributeValue("INFO", "PLAYTIME");

                if(String.IsNullOrEmpty(str)) return null;

                return System.Convert.ToInt32(str);
            }
        }

        public double? PlayTimeFloat
        {
            get
            {
                var str = GetAttributeValue("INFO", "PLAYTIME_FLOAT");

                if(String.IsNullOrEmpty(str)) return null;

                return System.Convert.ToDouble(str);
            }
        }



        #endregion

    }
}


