using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracktorTagger
{
    public class TracktorCollection
    {

      
           public IList<TraktorTrack> Entries {get; private set;}

           private System.Xml.XmlDocument _collectionXmlDoc;


           public string FileName { get; set; }


           public void SaveCollection()
           {
               _collectionXmlDoc.Save(FileName);
           }

        public TracktorCollection(string fileName)
        {
            Entries = new List<TraktorTrack>();
            FileName = fileName;

            _collectionXmlDoc = new System.Xml.XmlDocument();
            _collectionXmlDoc.Load(fileName);

            var entryNodes = _collectionXmlDoc.DocumentElement.SelectNodes("/NML/COLLECTION/ENTRY");


            var nmlNode = _collectionXmlDoc.DocumentElement.SelectNodes("/NML")[0];
            var versionString = nmlNode.Attributes["VERSION"].Value;

            if(versionString != "15") throw new InvalidOperationException("Unexpected NML version: " + versionString);





            foreach(System.Xml.XmlElement entryNode in entryNodes)
            {
                Entries.Add(new TraktorTrack(entryNode));
            }

           
        }

    }
}
