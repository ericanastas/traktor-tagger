using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorTagger
{
    /// <summary>
    /// Class used to read and modify the contents of a Traktor NML collection file.
    /// </summary>
    public class TracktorCollection
    {
        private List<TraktorTrack> _entries;
        private System.Xml.XmlDocument _collectionXmlDoc;


        /// <summary>
        /// Read only collection of the collection's entries
        /// </summary>
        public IList<TraktorTrack> Entries
        {
            get
            {
                return _entries.AsReadOnly();
            }
        }

        /// <summary>
        /// The collection's file name
        /// </summary>
        public string FileName { get; private set; }


        /// <summary>
        /// Saves any changes made to the track entries back the the NML file.
        /// </summary>
        public void SaveCollection()
        {
            _collectionXmlDoc.Save(FileName);
        }

        /// <summary>
        /// Opens a Traktor collection NML file
        /// </summary>
        /// <param name="fileName">The path to the collection NML file</param>
        public TracktorCollection(string fileName)
        {
            if(string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName");
            if(!System.IO.File.Exists(fileName)) throw new System.IO.FileNotFoundException("Traktor collection file not found", fileName);
            
            FileName = fileName;

            _collectionXmlDoc = new System.Xml.XmlDocument();
            _collectionXmlDoc.Load(fileName);

            //checks the NML Version
            var nmlNode = _collectionXmlDoc.DocumentElement.SelectNodes("/NML")[0];
            var versionString = nmlNode.Attributes["VERSION"].Value;
            if(versionString != "15") throw new InvalidOperationException("Unexpected NML version: " + versionString);

            var entryNodes = _collectionXmlDoc.DocumentElement.SelectNodes("/NML/COLLECTION/ENTRY");


            _entries = new List<TraktorTrack>();

            foreach(System.Xml.XmlElement entryNode in entryNodes)
            {
                _entries.Add(new TraktorTrack(entryNode));
            }
        }

    }
}
