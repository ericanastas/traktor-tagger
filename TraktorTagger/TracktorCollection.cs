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
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(TracktorCollection));
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
            log.Debug("SaveCollection() called");

            if(ReadOnly)
            {
                log.Debug("Collection is read only throwing InvalidOperationException");
                throw new InvalidOperationException("Collection is opened as read only");
            }


            log.Debug("Writing file...");
            _fileStream.Position = 0;
            _collectionXmlDoc.Save(_fileStream);

            log.Debug("Setting HasUnsavedChanges to FALSE");
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
            log.Debug("TraktorCollection(" + fileName + ", " + readOnly.ToString() + ")");

            if(string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName");
            if(!System.IO.File.Exists(fileName)) throw new System.IO.FileNotFoundException("Traktor collection file not found", fileName);
            

            FileName = fileName;
            ReadOnly = readOnly;


            log.Debug("Opening file stream: "+fileName);
            _fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);

            //creates new xml document
            _collectionXmlDoc = new System.Xml.XmlDocument();
            _collectionXmlDoc.Load(_fileStream);

            if(ReadOnly)
            {
                log.Debug("Catalog opened read only. Closing file stream...");
                _fileStream.Close();
                _fileStream = null;
            }

            //checks the NML Version
            log.Debug("Checking NML version");
            var nmlNode = _collectionXmlDoc.DocumentElement.SelectNodes("/NML")[0];
            var versionString = nmlNode.Attributes["VERSION"].Value;

            log.Debug("VERSION: "+versionString);

            if(versionString != "15")
            {
                log.Debug("Unexpected NML version. Throwing InvalidOperationException");
                throw new InvalidOperationException("Unexpected NML version: " + versionString);
            }


            //subscribes to change events
            log.Debug("Subscribing to xml node changed events");
            _collectionXmlDoc.NodeChanged += _collectionXmlDoc_NodeChanged;
            _collectionXmlDoc.NodeInserted += _collectionXmlDoc_NodeInserted;
            _collectionXmlDoc.NodeRemoved += _collectionXmlDoc_NodeRemoved;


            var entryNodes = _collectionXmlDoc.DocumentElement.SelectNodes("/NML/COLLECTION/ENTRY");

            log.Debug("Staring adding tracks for"+entryNodes.Count+" entries.");

            _entries = new List<TraktorTrack>();
            foreach(System.Xml.XmlElement entryNode in entryNodes)
            {
                log.Debug("Staring adding tracks for" + entryNodes.Count + " entries.");
                _entries.Add(new TraktorTrack(entryNode));
            }

            log.Debug("Completed adding tracks.");
        }

        void _collectionXmlDoc_NodeRemoved(object sender, System.Xml.XmlNodeChangedEventArgs e)
        {
            log.Debug("_collectionXmlDoc_NodeRemoved fired. Setting HasUnsavedChanges to TRUE");
            this.HasUnsavedChanges = true;
        }

        void _collectionXmlDoc_NodeInserted(object sender, System.Xml.XmlNodeChangedEventArgs e)
        {
            log.Debug("_collectionXmlDoc_NodeInserted fired. Setting HasUnsavedChanges to TRUE");
            this.HasUnsavedChanges = true;
        }

        void _collectionXmlDoc_NodeChanged(object sender, System.Xml.XmlNodeChangedEventArgs e)
        {
            log.Debug("_collectionXmlDoc_NodeChanged fired. Setting HasUnsavedChanges to TRUE");
            this.HasUnsavedChanges = true;
        }


        protected virtual void Dispose(bool disposing)
        {
            log.Debug("~Dispose("+disposing.ToString()+") called");

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
                log.Debug("_fileStream !+ null. Closing and setting to null.");
                _fileStream.Close();
                _fileStream = null;
            }
        }

        ~TracktorCollection()
        {
            log.Debug("~TracktorCollection() called");
            // Simply call Dispose(false).
            Dispose(false);
        }

        public void Dispose()
        {
            log.Debug("Dispose() called");
            Dispose(true);
            GC.SuppressFinalize(this); 
        }
    }
}
