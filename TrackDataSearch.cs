using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracktorTagger
{
    public abstract class TrackDataSearch:ITrackDataSearch
    {

        public TrackDataSearch(int totalResults, string query, ITrackDataSource source)
        {
            if(string.IsNullOrEmpty(query)) throw new ArgumentNullException("query");
            this.SearchQuery = query;

            if(totalResults < 1) throw new ArgumentException("totalResults");
            this.TotalResults = totalResults;

            if(source == null) throw new ArgumentNullException("source");
            this.DataSource = source;

        }

        public string SearchQuery
        {
            get;
            private set;
        }

        public int TotalResults
        {
            get;
            private set;
        }

        public ITrackDataSource DataSource
        {
            get;
            private set;
        }

        private List<TrackData> _results = new List<TrackData>();

        protected void AddResult(TrackData data)
        {
            this._results.Add(data);
        }

        public IList<TrackData> Results
        {
            get { return _results.AsReadOnly(); }
        }

        public abstract IList<TrackData> LoadMoreResults();
    }
}
