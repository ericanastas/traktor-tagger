using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorTagger
{
    public class PlaceHolderTrackDataSearch : ITrackDataSearch
    {
        private string _searchQuery;
        private int _total;
        private int _perpage;

        private int _loadedPages;

        public PlaceHolderTrackDataSearch(PlaceHolderTrackDataSource source, string searchQuery, int totalResults, int resultsPerPage)
        {
            this._searchQuery = searchQuery;

            this.Source = source;
            SearchQuery = searchQuery;

            this._total = totalResults;
            this._perpage = resultsPerPage;
            _loadedPages = 0;

            this._tracks = new List<TrackData>();

            LoadMoreResults();
        }

        public string SearchQuery
        {
            get;
            private set;
        }



        private List<TrackData> _tracks;
        public IList<TrackData> Results
        {
            get { return _tracks.AsReadOnly(); }
        }





        public IList<TrackData> LoadMoreResults()
        {

            List<TrackData> returnList = new List<TrackData>();





            for(int i = 1; i <= _perpage; i++)
            {

                int trackIdInt = _perpage * _loadedPages + i;

                string trackId = trackIdInt.ToString();

                if(trackIdInt > _total) break;

                TrackData data = new TrackData("PlaceHolderTrackDataSource",
       "id" + trackId,
       "artist " + trackId,
       "title " + trackId,
       "mix " + trackId,
       "remixer " + trackId,
       "release " + trackId,
       "producer " + trackId,
       "label " + trackId,
       "cat no " + trackId,
       "genre " + trackId,
        KeyEnum.A,
        DateTime.Now,
        new Uri(@"http://www.google.com"));

                returnList.Add(data);
                _tracks.Add(data);

            }

            if(returnList.Count > 0) _loadedPages++;



            return returnList;

        }

        public ITrackDataSource Source
        {
            get;
            private set;
        }




        public bool HasMoreResults
        {
            get
            {

                if(_tracks.Count < _total) return true;
                else return false;
            }
        }
    }
}
