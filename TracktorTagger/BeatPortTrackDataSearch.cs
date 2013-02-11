using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TraktorTagger
{
    public class BeatPortTrackDataSearch : ITrackDataSearch
    {

        string _query;
        private int _trackPerPage;
        private int _currentPage;
        private int _totalPages;


        private List<TrackData> _trackData;

        private Dictionary<int, Dictionary<string, dynamic>> _releaseDataCache = new Dictionary<int, Dictionary<string, dynamic>>();



        public string SearchQuery
        {
            get { return _query; }
        }

        public ITrackDataSource Source
        {
            get;
            private set;
        }


        public IList<TrackData> Results
        {
            get { return _trackData.AsReadOnly(); }
        }

        public bool HasMoreResults
        {
            get;
            private set;
        }



        public BeatPortTrackDataSearch(BeatportTrackDataSource source, Uri searchUri)
        {

            if(source == null) throw new ArgumentNullException("source");
            if(searchUri == null) throw new ArgumentNullException("searchUri");

            this.Source = source;
            this.HasMoreResults = false; //sets HasMoreResults to false initaly
            this._query = searchUri.AbsoluteUri;


            //Beatport.com URL regex
            System.Text.RegularExpressions.Regex trackURIRegex = new System.Text.RegularExpressions.Regex(@"^http://www.beatport.com/track/(.*)/(\d*)$");
            System.Text.RegularExpressions.Regex releaseURIRegex = new System.Text.RegularExpressions.Regex(@"^http://www.beatport.com/release/(.*)/(\d*)$");


            //attempts to match URL
            var trackMatch = trackURIRegex.Match(searchUri.AbsoluteUri);
            var releaseMatch = releaseURIRegex.Match(searchUri.AbsoluteUri);


            if(trackMatch.Success)
            {
                int trackId = System.Convert.ToInt32(trackMatch.Groups[2].Value);

                var trackResults = GetTrackData(trackId);

                var trackData = trackResults["results"]["track"];
                var releaseData = trackResults["results"]["release"];

                var track = ParseResponse(trackData, releaseData, this.Source.Host);

                _trackData = new List<TrackData>();
                _trackData.Add(track);
            }
            else if(releaseMatch.Success)
            {
                int releaseId = System.Convert.ToInt32(releaseMatch.Groups[2].Value);

                var releaseResults = GetReleaseData(releaseId);


                _trackData = new List<TrackData>();

                var releaseData = releaseResults["results"]["release"];

                foreach(Dictionary<string, dynamic> trackData in releaseResults["results"]["tracks"])
                {
                    var newTrack = ParseResponse(trackData, releaseData,this.Source.Host);

                    _trackData.Add(newTrack);
                }



    
            }
            else
            {
                throw new ArgumentException("Invalid Beatport track URL format", "searchUri");
            }
        }


        public BeatPortTrackDataSearch(BeatportTrackDataSource source, string query, int trackPerPage)
        {
            HasMoreResults = true;
            _query = query;
            _trackPerPage = trackPerPage;
            _currentPage = 1;
            _trackData = new List<TrackData>();

            Source = source;


            //loads the inital resuts
            LoadMoreResults();
        }


        public IList<TrackData> LoadMoreResults()
        {
            //if all results are loaded just return an empty list
            if(!HasMoreResults) throw new InvalidOperationException("Can not load more results. BeatportTrackDataSearch.HasMoreResults == false");


            var trackSearchData = GetSearchResultsData(_query, _currentPage, _trackPerPage);

            _totalPages = trackSearchData["metadata"]["totalPages"];


            List<TrackData> newTracks = new List<TrackData>();


            foreach(Dictionary<string, dynamic> trackData in trackSearchData["results"])
            {
                int releaseId = trackData["release"]["id"];

                Dictionary<string, dynamic> releaseData;


                if(_releaseDataCache.ContainsKey(releaseId))
                {
                    releaseData = _releaseDataCache[releaseId];
                }
                else
                {
                    releaseData = GetReleaseData(releaseId)["results"]["release"];
                    _releaseDataCache.Add(releaseId, releaseData);
                }

                TrackData newTrack = ParseResponse(trackData, releaseData, this.Source.Host);
                newTracks.Add(newTrack);
            }

            //adds the new tracks to the full list
            _trackData.AddRange(newTracks);


            //check if at the last page
            if(_currentPage == _totalPages)
            {
                HasMoreResults = false;
            }
            else
            {
                _currentPage++;
            }

            return newTracks.AsReadOnly();
        }


        #region Parsing Methods

        private static string GetArtist(System.Collections.ArrayList artists, string type)
        {
            List<string> artistNames = new List<string>();

            foreach(Dictionary<string, dynamic> artist in artists)
            {
                string artistType = (string)artist["type"];

                if(artistType == type)
                {
                    artistNames.Add(artist["name"]);
                }
            }

            string artistStr = string.Join(", ", artistNames);

            return artistStr;

        }

        private static Uri GetUri(int id, string slub)
        {
            UriBuilder builder = new UriBuilder("http://www.beatport.com");

            builder.Path = "track/" + slub + "/" + id.ToString();

            return builder.Uri;
        }

        private static DateTime ParseReleaseDate(string dateStr)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^(\d{4})-(\d{2})-(\d{2})$");

            var m = regex.Match(dateStr);

            if(!m.Success) throw new InvalidOperationException("Error reading date string:" + dateStr);


            int year = System.Convert.ToInt32(m.Groups[1].Value);
            int month = System.Convert.ToInt32(m.Groups[2].Value);
            int day = System.Convert.ToInt32(m.Groups[3].Value);


            return new DateTime(year, month, day);
        }

        private static KeyEnum GetKey(dynamic key)
        {
            StringBuilder keyStringBuilder = new StringBuilder();

            keyStringBuilder.Append(key["standard"]["letter"]);

            if(System.Convert.ToBoolean(key["standard"]["sharp"]))
            {
                keyStringBuilder.Append("#");
            }
            else if(System.Convert.ToBoolean(key["standard"]["flat"]))
            {
                keyStringBuilder.Append("b");
            }

            if(key["standard"]["chord"] == "minor")
            {
                keyStringBuilder.Append("m");
            }

            var keyString = keyStringBuilder.ToString();

            KeyEnum returnKey = KeyEnumStringConverter.ConvertFromString(keyString);

            return returnKey;


        }

        private static string GetGenre(System.Collections.ArrayList genres)
        {
            List<string> genreNames = new List<string>();

            foreach(Dictionary<string, dynamic> genre in genres)
            {
                genreNames.Add((string)genre["name"]);
            }

            string genreStr = string.Join(", ", genreNames);

            return genreStr;
        }

        private static TrackData ParseResponse(Dictionary<string, dynamic> trackData, Dictionary<string, dynamic> releaseData, string host)
        {
            string trackId = trackData["id"].ToString();
            string title = trackData["name"];
            string mix = trackData["mixName"];
            string label = trackData["label"]["name"];
            DateTime releaseDate = ParseReleaseDate(trackData["releaseDate"]);
            string release = trackData["release"]["name"];
            Uri url = GetUri(trackData["id"], trackData["slug"]);
            string genre = GetGenre(trackData["genres"]);
            string artist = GetArtist(trackData["artists"], "artist");
            string remixer = GetArtist(trackData["artists"], "remixer");
            KeyEnum key = GetKey(trackData["key"]);


            string catalogNo = releaseData["catalogNumber"]; //catalog number is the only value I need the releaseData for
            string producer = null; //not sure how to get the producer out of beatport


            TrackData track = new TrackData(host,
                trackId,
                artist,
                title,
                mix,
                remixer,
                release,
                producer,
                label,
                catalogNo,
                genre,
                key,
                releaseDate,
                url);

            return track;



        }


        #endregion


        #region Data Retrival Methods

        /// <summary>
        /// Returns results for querying a specific track id 
        /// </summary>
        /// <param name="trackId"></param>
        /// <returns></returns>
        private static Dictionary<string, dynamic> GetTrackData(int trackId)
        {
            Dictionary<String, dynamic> returnDict;

            using(var webclient = new WebClient())
            {
                System.UriBuilder trackDataUrlBuilder = new UriBuilder("http:", "api.beatport.com");
                trackDataUrlBuilder.Path = "catalog/3/beatport/track";

                trackDataUrlBuilder.Query = "id=" + trackId.ToString();


                string jsonString = webclient.DownloadString(trackDataUrlBuilder.Uri.AbsoluteUri);

                var jss = new JavaScriptSerializer();
                returnDict = jss.Deserialize<Dictionary<string, dynamic>>(jsonString);
            }

            return returnDict;
        }


        private static Dictionary<string, dynamic> GetReleaseData(int releaseId)
        {
            Dictionary<String, dynamic> returnDict;

            using(var webclient = new WebClient())
            {
                System.UriBuilder trackDataUrlBuilder = new UriBuilder("http:", "api.beatport.com");
                trackDataUrlBuilder.Path = "catalog/3/beatport/release";

                trackDataUrlBuilder.Query = "id=" + releaseId.ToString();


                string jsonString = webclient.DownloadString(trackDataUrlBuilder.Uri.AbsoluteUri);

                var jss = new JavaScriptSerializer();
                returnDict = jss.Deserialize<Dictionary<string, dynamic>>(jsonString);
            }

            return returnDict;


        }


        private static Dictionary<string, dynamic> GetSearchResultsData(string searchQuery, int page, int tracksPerPage)
        {
            Dictionary<String, dynamic> returnDict;

            using(var webclient = new WebClient())
            {
                System.UriBuilder trackDataUrlBuilder = new UriBuilder("http:", "api.beatport.com");
                trackDataUrlBuilder.Path = "catalog/3/search";

                string query = "query=" + searchQuery + "&page=" + page + "&facets[]=fieldType:track&perPage=" + tracksPerPage.ToString();

                trackDataUrlBuilder.Query = query;

                string jsonString = webclient.DownloadString(trackDataUrlBuilder.Uri.AbsoluteUri);

                var jss = new JavaScriptSerializer();
                returnDict = jss.Deserialize<Dictionary<string, dynamic>>(jsonString);
            }

            return returnDict;
        }




    }

        #endregion
}




