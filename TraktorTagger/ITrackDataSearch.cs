using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorTagger
{
    public interface ITrackDataSearch
    {
        string SearchQuery { get; }

        /// <summary>
        /// Full list of track results from the search
        /// </summary>
        /// <remarks></remarks>
        IList<TrackData> Results { get; }


        /// <summary>
        /// 
        /// </summary>
        bool HasMoreResults { get; }
        

        /// <summary>
        /// Loads more results
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If HasMoreResults is false.</exception>
        IList<TrackData> LoadMoreResults();


        /// <summary>
        /// The ITrackDataSource which provided the TrackDataSearch
        /// </summary>
        ITrackDataSource Source { get; }
    }
}
