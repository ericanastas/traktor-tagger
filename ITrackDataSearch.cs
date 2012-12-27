using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracktorTagger
{
    public interface ITrackDataSearch : IDisposable
    {
        string SearchQuery { get; }
        int TotalResults { get; }

        int ResultsPerPage { get; }



        IList<TrackData> Results { get; }


        IList<TrackData> LoadMoreResults();
    }
}
