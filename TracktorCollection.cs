using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracktorTagger
{
    public class TracktorCollection
    {

      
           public IList<TracktorEntry> Entries {get; private set;}

           private System.Xml.XmlDocument _collectionXmlDoc;


           public string FileName { get; set; }


           public void SaveCollection()
           {
               _collectionXmlDoc.Save(FileName);
           }

        public TracktorCollection(string fileName)
        {
            Entries = new List<TracktorEntry>();
            FileName = fileName;

            _collectionXmlDoc = new System.Xml.XmlDocument();
            _collectionXmlDoc.Load(fileName);

            var entryNodes = _collectionXmlDoc.DocumentElement.SelectNodes("/NML/COLLECTION/ENTRY");





            foreach(System.Xml.XmlElement entryNode in entryNodes)
            {
                Entries.Add(new TracktorEntry(entryNode));
            }

           
        }

    }
}
