using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracktorTagger
{
    public interface ITrackDataSource
    {
        IEnumerable<TrackData> SearchTracks(string searchQuery);
        TrackData GetTrack(string trackId);

    }
}
