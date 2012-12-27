using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracktorTagger
{

    public class PlaceHolderTrackDataSearch : ITrackDataSearch
    {
        private string _searchQuery;

        public PlaceHolderTrackDataSearch(string searchQuery)
        {
            this._searchQuery = searchQuery;
        }

        public string SearchQuery
        {
            get { return _searchQuery; }
        }

        public int TotalResults
        {
            get { throw new NotImplementedException(); }
        }

        public int ResultsPerPage
        {
            get { throw new NotImplementedException(); }
        }

        public IList<TrackData> Results
        {
            get { throw new NotImplementedException(); }
        }

        public IList<TrackData> LoadMoreResults()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class PlaceHolderTrackDataSource : ITrackDataSource
    {

        public PlaceHolderTrackDataSource(int total)
        {
            _total = total;
        }

        private int _total;



        public TrackData GetTrack(string trackId)
        {
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
                "lyrics " + trackId,
                "genre " + trackId,
                 new Key('A', Accidental.Flat, Chord.Major),
                 DateTime.Now,
                 new Uri(@"http://www.google.com"));

            return data;
        }

  

        public string Name
        {
            get { return "test data source"; }
        }


        ITrackDataSearch ITrackDataSource.SearchTracks(string searchQuery)
        {
            throw new NotImplementedException();
        }
    }
}
