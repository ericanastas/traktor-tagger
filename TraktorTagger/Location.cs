using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorTagger
{
    /// <summary>
    /// Location of the audio file of a TraktorTrack 
    /// </summary>
    public class Location
    {
        public string FilePath { get; private set; }
        public string Directory { get; private set; }
        public string FileName { get; private set; }
        public string Volume { get; private set; }
        public string VolumeId { get; private set; }
    }
}
