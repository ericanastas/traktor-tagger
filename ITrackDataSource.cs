using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracktorTagger
{
    public interface ITrackDataSource
    {
        string Name { get; }

        IEnumerable<TrackData> SearchTracks(string searchQuery);

        int GetTotalResultCount(string searchQuery);

        TrackData GetTrack(string trackId);
    }


    public interface ITrackDataSearchResults : IDisposable
    {
        string SearchQuery { get; }
        int TotalResults { get; }
        int LoadedResults {get;}
        int ResultsPerPage { get; }

        List<TrackData> Results {get;}

        List<TrackData> LoadResults(int count);    
    }
}
