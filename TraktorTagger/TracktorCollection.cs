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
    public class TracktorCollection:IDisposable
    {
        
        private List<TraktorTrack> _entries;
        private System.Xml.XmlDocument _collectionXmlDoc;

        private System.IO.FileStream _fileStream;

        

        public bool ReadOnly { get; private set; }


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
            if(ReadOnly) throw new InvalidOperationException("Collection is opened as read only");


            _fileStream.Position = 0;

            _collectionXmlDoc.Save(_fileStream);

            this.HasUnsavedChanges = false;
        }

        public bool HasUnsavedChanges
        {
            get;
            private set;
        }

        /// <summary>
        /// Opens a Traktor collection NML file
        /// </summary>
        /// <param name="fileName">The path to the collection NML file</param>
        public TracktorCollection(string fileName, bool readOnly)
        {
            if(string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName");
            if(!System.IO.File.Exists(fileName)) throw new System.IO.FileNotFoundException("Traktor collection file not found", fileName);
            
            FileName = fileName;

            ReadOnly = readOnly;


            _fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.Read);

            

            //creates new xml document
            _collectionXmlDoc = new System.Xml.XmlDocument();
            _collectionXmlDoc.Load(_fileStream);

            if(ReadOnly)
            {
                _fileStream.Close();
                _fileStream = null;
            }



            //subscribes to change events
            _collectionXmlDoc.NodeChanged += _collectionXmlDoc_NodeChanged;
            _collectionXmlDoc.NodeInserted += _collectionXmlDoc_NodeInserted;
            _collectionXmlDoc.NodeRemoved += _collectionXmlDoc_NodeRemoved;


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

        void _collectionXmlDoc_NodeRemoved(object sender, System.Xml.XmlNodeChangedEventArgs e)
        {
            this.HasUnsavedChanges = true;
        }

        void _collectionXmlDoc_NodeInserted(object sender, System.Xml.XmlNodeChangedEventArgs e)
        {
            this.HasUnsavedChanges = true;
        }

        void _collectionXmlDoc_NodeChanged(object sender, System.Xml.XmlNodeChangedEventArgs e)
        {
            this.HasUnsavedChanges = true;
        }


        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                // Free other state (managed objects).
            }
            // Free your own state (unmanaged objects).
            // Set large fields to null.


            _collectionXmlDoc.NodeChanged -= _collectionXmlDoc_NodeChanged;
            _collectionXmlDoc.NodeInserted -= _collectionXmlDoc_NodeInserted;
            _collectionXmlDoc.NodeRemoved -= _collectionXmlDoc_NodeRemoved;

            if(_fileStream != null)
            {
                _fileStream.Close();
                _fileStream = null;
            }
        }

        ~TracktorCollection()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); 
        }
    }
}
