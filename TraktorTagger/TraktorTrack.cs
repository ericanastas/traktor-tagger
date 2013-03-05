using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace TraktorTagger
{
    /// <summary>
    /// A track in a Traktor catalog NML file
    /// </summary>
    /// <remarks>Returned from the TracktorCollection class</remarks>
    public class TraktorTrack : System.ComponentModel.INotifyPropertyChanged
    {
        private System.Xml.XmlElement entryNode;

        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(TraktorTrack));

        /// <summary>
        /// Constructor to create a new TracktorTrack
        /// </summary>
        /// <param name="entryNode">An entry node from the collection NML file</param>
        internal TraktorTrack(System.Xml.XmlElement entryNode)

        {
            if(entryNode == null)
            {
                log.Error("TraktorTrack constructor passed null node parameter. Throwing ArgumentNullException...");
                throw new ArgumentNullException("entryNode");
            }
            this.entryNode = entryNode;
        }

        #region XML Methods

        /// <summary>
        /// Returns an attribute value from the entry node, or one of its child elements.
        /// </summary>
        /// <param name="elementName">The child element of the Entry element. If NULL then the Entry element is used.</param>
        /// <param name="attributeName">The name of the attribute. Can not be empty of null.</param>
        /// <returns>The value of the attribute, or NULL if the specified attribute or element can not be found.</returns>
        private string GetAttributeValue(string elementName, string attributeName)
        {
            log.Debug("GetAttributeValue(" + elementName + ", " + attributeName + ");");
            log.Debug("Entry Node XML:" + entryNode.OuterXml);

            System.Xml.XmlNode node;

            if(string.IsNullOrEmpty(attributeName)) throw new ArgumentNullException("attributeName");

            if(String.IsNullOrEmpty(elementName))
            {
                log.Debug("No  child element specified, using entry element.");
                node = entryNode;
            }
            else
            {
                node = entryNode.SelectSingleNode("./" + elementName);

                if(node == null)
                {
                    log.Debug("No  child "+elementName+" element found. Returning NULL");
                    return null;
                }
            }

            var att = node.Attributes[attributeName];

            if(att == null)
            {
                log.Debug("No " + attributeName + " attribute found. Returning NULL");
                return null;
            }
            else
            {
                log.Debug("Returning "+attributeName+" value: "+att.Value);
                return att.Value;
            }
        }

        /// <summary>
        /// Removes an attribute from the entry node or one of its child elements.
        /// </summary>
        /// <param name="elementName">The child element of the Entry element. If NULL then the Entry element is used.</param>
        /// <param name="attributeName">The name of the attribute. Can not be empty of null.</param>
        private void RemoveAttribute(string elementName, string attributeName)
        {
            log.Debug("RemoveElement(" + elementName + ", "+attributeName+")");
            log.Debug("Entry Node XML:" + entryNode.OuterXml);

            System.Xml.XmlNode node;

            if(string.IsNullOrEmpty(attributeName)) throw new ArgumentNullException("attributeName");

            if(String.IsNullOrEmpty(elementName))
            {
                log.Debug("No  child element specified, using entry element.");
                node = entryNode;
            }
            else
            {
                node = entryNode.SelectSingleNode("./" + elementName);

                if(node == null)
                {
                    log.Debug("No  child "+elementName+" element found. Returning...");
                    return;
                }
            }
            var att = node.Attributes[attributeName];

            if(att != null)
            {
                node.Attributes.Remove(att);
                log.Debug(attributeName + " attribute found and removed.");
            }
            else
            {
                log.Debug("No existing "+attributeName+" attribute found to remove");
            }
        }


        /// <summary>
        /// Removes a child element from the entry element
        /// </summary>
        /// <param name="elementName">The element to remove</param>
        private void RemoveElement(string elementName)
        {
            log.Debug("RemoveElement(" + elementName + ")");
            log.Debug("Entry Node XML:" + entryNode.OuterXml);
            

            if(String.IsNullOrEmpty(elementName)) throw new ArgumentNullException("elementName");

            System.Xml.XmlNode node = entryNode.SelectSingleNode("./" + elementName);

            if(node == null)
            {
                log.Debug(elementName+" node was not found");
            }
            else
            {
                entryNode.RemoveChild(node);
                log.Debug(elementName + " node found and removed.");
            }
        }


        /// <summary>
        /// Sets an attribute value on the entry element, or one of its child elements.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="elementName">The child element of the entry element to set the attribute on. If NULL then the attribute is set on the entry element</param>
        /// <param name="attributeName">The name of the attribute to set. Can not be NULL</param>
        /// <param name="value">The value to set. Can not be an empty string.</param>
        private void SetAttributeValue(string elementName, string attributeName, string value)
        {

            log.Debug("SetAttributeValue(" + elementName + ", " + attributeName + ", " + value + ")");

            System.Xml.XmlNode node;

            if(string.IsNullOrEmpty(attributeName)) throw new ArgumentNullException("attributeName");
            if(value == null) throw new ArgumentNullException("value");

            log.Debug("Entry Node XML:" + entryNode.OuterXml);

            if(String.IsNullOrEmpty(elementName))
            {
                log.Debug("No  child element specified, using entry element.");
                node = entryNode;
            }
            else
            {
                node = entryNode.SelectSingleNode("./" + elementName);

                if(node == null)
                {
                    log.Debug(elementName + " child element not found. Creating new element.");
                    node = entryNode.OwnerDocument.CreateElement(elementName);
                    entryNode.AppendChild(node);
                }
                else
                {
                    log.Debug("Existing " + elementName + " element found");
                }
            }

            var att = node.Attributes[attributeName];

            if(att == null)
            {
                log.Debug("Existing "+attributeName+" attribute not found. Creating new attribtue");

                var newAtt = node.OwnerDocument.CreateAttribute(attributeName);
                newAtt.Value = value;
                log.Debug("Setting attribute to: " + value);

                node.Attributes.Append(newAtt);
            }
            else
            {
                log.Debug("Existing " + attributeName + " attribute found.");
                log.Debug("Setting attribute to: "+value);
                att.Value = value;
            }
        }

        #endregion



        /// <summary>
        /// Uri to page with track data used to tag track
        /// </summary>
        /// <remarks>This should be the humman readable WWW page and not and API resource</remarks>
        public Uri DataSourceUri
        {
            get
            {
                if(Uri.IsWellFormedUriString(Comment2, UriKind.Absolute))
                {
                    Uri uri = new Uri(Comment2);
                    return uri;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if(value != null)
                {
                    if(value.IsAbsoluteUri)
                    {
                        Comment2 = value.AbsoluteUri;
                    }
                    else
                    {
                        throw new InvalidOperationException("DataSource URI can not be set to a relative URI");
                    }
                }
                else
                {
                    Comment2 = null;
                }

                OnPropertyChanged("DataSourceUri");
                OnPropertyChanged("DataSourceHostName");
                OnPropertyChanged("Comment2");

            }
        }


        /// <summary>
        /// Host of the datasource used to tag the track
        /// </summary>
        /// <remarks>This value is pulled automaticaly from the value of DataSourceUri</remarks>
        public string DataSourceHostName
        {
            get
            {
                if(DataSourceUri != null)
                {
                    return DataSourceUri.Host;
                }
                else
                {
                    return null;
                }
            }

        }




        #region String Properties



        /// <summary>
        /// Track title
        /// </summary>
        /// <remarks>Available when track is first imported. Can not be set to NULL or an empty string.</remarks>
        public string Title
        {
            //Title Attribute is always there;
            get
            {
                return GetAttributeValue(null, "TITLE");
            }
            set
            {
                if(!String.IsNullOrEmpty(value))
                {
                    SetAttributeValue(null, "TITLE", value);
                    OnPropertyChanged("Title");
                }
                else
                {

                }
            }
        }


        /// <summary>
        /// Track artist
        /// </summary>
        /// <remarks>ARTIST attribute is removed when value is set to NULL or empty string</remarks>
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
        /// Track mix
        /// </summary>
        /// <remarks>MIX attribute is removed when value is set to NULL or empty string</remarks>
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
        /// Track producer
        /// </summary>
        /// <remarks>PRODUCER attribute is removed when value is set to NULL or a empty string</remarks>
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
        /// Track remixer
        /// </summary>
        /// <remarks>REMIXER attribtute is removed when set to NULL or a empty string.</remarks>
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
        /// Track release
        /// </summary>
        /// <remarks>ALBUM element is removed when set to NULL or a empty string.</remarks>
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


        /// <summary>
        /// Track lyrics
        /// </summary>
        /// <remarks>KEY_LYRICS attribtute is removed when set to NULL or a empty string.</remarks>
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
        /// Track label
        /// </summary>
        /// <remarks>LABEL attribtute is removed when set to NULL or a empty string.</remarks>
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


        /// <summary>
        /// Track catalog number
        /// </summary>
        /// <remarks>CATALOG_NO attribtute is removed when set to NULL or a empty string.</remarks>
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
        /// Track genre
        /// </summary>
        /// <remarks>GENRE attribtute is removed when set to NULL or a empty string.</remarks>
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
        /// Track comments
        /// </summary>
        /// <remarks>Comment1 attribtute is removed when set to NULL or a empty string.</remarks>
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
        /// Track comments
        /// </summary>
        /// <remarks>RATING attribtute is removed when set to NULL or a empty string.</remarks>
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
                OnPropertyChanged("DataSourceUri");
                OnPropertyChanged("DataSourceHostName");
            }
        }


        #endregion



        /// <summary>
        /// Track key
        /// </summary>
        /// <remarks>KEY attribtute is removed when set to NULL</remarks>
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
        /// The date/time the entry in the catalog was last modified
        /// </summary>
        /// <remarks>Available when track is first imported</remarks>
        public DateTime Modified
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
                //In the future I may want this to be set when TraktorTagger modifies the collection
                throw new NotImplementedException();
            }

        }



        /// <summary>
        /// Track BPM
        /// </summary>
        /// <remarks>TEMPO element is removed when set to NULL. Throws an ArgumentOutOfRangeException if set to a negative number.</remarks>
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
                    if(value < 0) throw new ArgumentOutOfRangeException("BPM", "BPM can not be set to a negative number.");

                    SetAttributeValue("TEMPO", "BPM", value.ToString());
                    SetAttributeValue("TEMPO", "BPM_QUALITY", "100");
                }
                else
                {
                    RemoveElement("TEMPO");
                }
            }

        }

        /// <summary>
        /// Track Rating
        /// </summary>
        /// <remarks>RANKING attribute is removed when set to NULL.</remarks>
        public Rating? Rating
        {
            get
            {
                var ratingStr = GetAttributeValue("INFO", "RANKING");

                if(ratingStr == null) return null;

                int ratingValInt = System.Convert.ToInt32(ratingStr);

                //converts 0-255 rating into enum int value
                Decimal value = Math.Round((decimal)ratingValInt / (decimal)255 * (decimal)5);
                int ratingEnumInt = System.Convert.ToInt32(value);

                return (Rating)ratingEnumInt;
            }
            set
            {
                if(value.HasValue)
                {
                    int ratingValueInt = ((int)value) * 51;

                    SetAttributeValue("INFO", "RANKING", ratingValueInt.ToString());
                }
                else
                {
                    RemoveAttribute("INFO", "RANKING");
                }

                OnPropertyChanged("Rating");
            }
        }



        /// <summary>
        /// Track release date
        /// </summary>
        /// <remarks>RELEASE_DATE attribute is removed when set to NULL.</remarks>
        public DateTime? Released
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

                OnPropertyChanged("Released");
            }
        }


        public override string ToString()
        {
            return this.Title + " " + this.Mix + " " + this.Artist + " " + this.Label;
        }


        #region Propertise that need work

        /// <summary>
        /// Location of the track audio file.
        /// </summary>
        /// <remarks>Available when track is first imported</remarks>
        public string FileName
        {
            get
            {
                return System.IO.Path.GetFileName(FilePath);
            }
        }


        public string FilePath
        {
            //<LOCATION DIR="/:Beatport/:" FILE="Agora - Soul Budda (Deep Tribal Dub) - Slopshop Records.wav" VOLUME="E:" VOLUMEID="96c1318d">

            get
            {
                string dir = GetAttributeValue("LOCATION", "DIR");
                string file = GetAttributeValue("LOCATION", "FILE");
                string vol = GetAttributeValue("LOCATION", "VOLUME");

                if(String.IsNullOrEmpty(dir) || String.IsNullOrEmpty(file) || String.IsNullOrEmpty(vol))
                {
                    return null;
                }

                string pathStr = vol + dir.Replace("/:", "\\") + file;

                return pathStr;
            }
            set
            {
                log.Debug("Setting TraktorTrak.FilePath to " + value);

                throw new NotImplementedException("Can't set FilePath. Need to figure out how to set the correct VOLUMEID value");

                

                if(System.IO.File.Exists(value))
                {
                    log.Debug("File was found: " + value);
                    string root = System.IO.Path.GetPathRoot(value);
                    string file = System.IO.Path.GetFileName(value);


                    log.Debug("FILE: " + file);

                    var volRegex = new System.Text.RegularExpressions.Regex(@"^[A-Z]:\\");
                    var volMatch = volRegex.Match(root);

                    if(volMatch.Success)
                    {
                        string vol = root.Substring(0, 2);

                        string dir = System.IO.Path.GetDirectoryName(value).Substring(2) + "\\";

                        dir = dir.Replace("\\", "/:");


                        string volumeId = string.Empty;


                        System.Management.ManagementObjectSearcher ms3 = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
                        foreach(ManagementObject mo in ms3.Get())
                        {
                            string id = (string)mo["DeviceID"];

                            if(id == vol)
                            {
                                volumeId = (string)mo["VolumeSerialNumber"];
                                break;
                            }
                        }

                        if(string.IsNullOrEmpty(volumeId))
                        {
                            string m = "Could not determine the VOLUMEID of " + vol;
                            log.Error(m);
                            throw new InvalidOperationException("Could not determine the VOLUMEID of " + vol);
                        }
                        

                        log.Debug("Setting location attributes...");
                        log.Debug("FILE: " + file);
                        log.Debug("VOLUME: " + vol);
                        log.Debug("DIR: " + dir);
                        log.Debug("VOLUMEID: " + volumeId);


                        SetAttributeValue("LOCAL", "FILE", file);
                        SetAttributeValue("LOCAL", "VOLUME", vol);
                        SetAttributeValue("LOCAL", "DIR", dir);
                        SetAttributeValue("LOCAL", "VOLUMEID", volumeId);

                    }
                    else
                    {
                        throw new InvalidOperationException("Path volume not in format C:\\");
                    
                    }
                }
                else
                {
                    log.Debug("File was not found: "+ value);
                    throw new FileNotFoundException("Could not find the file", value);
                }


            }
        }

        #endregion

   

        #region Read Only Properties

        /// <summary>
        /// Date the track was imported into the catalog
        /// </summary>
        /// <remarks>Available when track is first imported</remarks>
        public DateTime Imported
        {
            get
            {
                return DateTime.Parse(GetAttributeValue("INFO", "IMPORT_DATE"));
            }
        }


        /// <summary>
        /// Last date the track was played
        /// </summary>
        public DateTime? LastPlayed
        {
            get
            {
                var lastStr = GetAttributeValue("INFO", "LAST_PLAYED");

                if(string.IsNullOrEmpty(lastStr))
                {
                    return null;
                }
                else
                {
                    return DateTime.Parse(lastStr);
                }
            }
        }

        /// <summary>
        /// Size of the audio file in KB
        /// 
        /// Note: The values found in the NML file do not seem to match the actual files?
        /// </summary>
        /// 
        /// <remarks>Available when track is first imported</remarks>
        public long FileSize
        {
            get
            {
                string sizeStr = GetAttributeValue("INFO", "FILESIZE");
                if(sizeStr == null) return 0;
                return System.Convert.ToInt64(sizeStr) * 1024;
            }
        }

        /// <summary>
        /// Bit rate of the audio file in kbps
        /// </summary>
        public double BitRate
        {
            get
            {
                string bitRateStr = GetAttributeValue("INFO", "BITRATE");
                if(bitRateStr == null) return 0;
                return System.Convert.ToDouble(bitRateStr)/1000.0;
            }
        }

        /// <summary>
        /// Number of times the track has been played in traktor
        /// </summary>
        public int PlayCount
        {
            get
            {
                var str = GetAttributeValue("INFO", "PLAYCOUNT");

                if(String.IsNullOrEmpty(str)) return 0;

                return System.Convert.ToInt32(str);
            }
        }

        /// <summary>
        /// Track length as a time span
        /// </summary>
        public TimeSpan? PlayTimeSpan
        {
            get
            {
                if(PlayTime == null)
                {
                    return null;
                }
                else
                {
                    var span = new TimeSpan((long)10000000 * System.Convert.ToInt64(PlayTime));
                    return span;
                }
            }
        }

        /// <summary>
        /// Track length in seconds
        /// </summary>
        public double? PlayTime
        {
            get
            {
                var str = GetAttributeValue("INFO", "PLAYTIME_FLOAT");

                if(String.IsNullOrEmpty(str)) return null;

                return System.Convert.ToDouble(str);
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

    }
}


