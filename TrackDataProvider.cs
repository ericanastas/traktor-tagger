using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracktorTagger
{
    public interface ITrackDataProvider
    {
        IEnumerable<Track> SearchTracks(string searchQuery);
        Track GetTrack(string trackId);

    }
}
