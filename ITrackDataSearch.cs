using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracktorTagger
{
    public interface ITrackDataSearch
    {
        string SearchQuery { get; }

        bool HasMoreResults { get; }

        IList<TrackData> Results { get; }


        IList<TrackData> LoadMoreResults();


        ITrackDataSource DataSource { get; }
    }
}
