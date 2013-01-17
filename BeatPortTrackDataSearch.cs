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
        Dictionary<int, string> _catalogNumberCache = new Dictionary<int, string>();
        List<TrackData> _trackData;


        public BeatPortTrackDataSearch(BeatportTrackDataSource source, string query, int trackPerPage)
        {
            HasMoreResults = true;
            _query = query;
            _trackPerPage = trackPerPage;
            _currentPage = 1;
            _trackData = new List<TrackData>();

            DataSource = source;

            LoadMoreResults();
        }


        public string SearchQuery
        {
            get { return _query; }
        }

        public ITrackDataSource DataSource
        {
            get;
            private set;
        }


        public IList<TrackData> Results
        {
            get { return _trackData.AsReadOnly(); }
        }



        public IList<TrackData> LoadMoreResults()
        {
            //if all results are loaded just return an empty list
            if(!HasMoreResults) return new List<TrackData>();

            var data = GetTrackData(SearchQuery, _currentPage, _trackPerPage);

            List<TrackData> newTracks = new List<TrackData>();


            foreach(Dictionary<string, dynamic> result in data["results"])
            {
                string trackId = result["id"].ToString();
                string title = result["name"];
                string mix = result["mixName"];
                string label = result["label"]["name"];

                //done
                DateTime releaseDate = GetReleaseDate(result["releaseDate"]);
                string release = result["release"]["name"];
                string catalogNo = GetCatalogNumber(result["release"]["id"]);
                Uri url = GetUri(result["id"], result["slug"]);
                string genre = GetGenre(result["genres"]);
                string artist = GetArtist(result["artists"], "artist");
                string remixer = GetArtist(result["artists"], "remixer");


                //not sure how to get the producer out of beatport
                string producer = null;

                Key key = GetKey(result["key"]);

                
                

                TrackData track = new TrackData(DataSource.HostName, trackId, artist, title, mix, remixer, release, producer, label, catalogNo, genre, key, releaseDate, url);

                newTracks.Add(track);
            }

            _trackData.AddRange(newTracks);



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

        

        private string GetCatalogNumber(int p)
        {

            if(_catalogNumberCache.ContainsKey(p))
            {
                return _catalogNumberCache[p];
            }

            String cat;

            using(var wc = new WebClient())
            {


                UriBuilder builder = new UriBuilder(@"http://api.beatport.com");

                builder.Path = "/catalog/3/beatport/release";

                   

                    builder.Query = "id="+p.ToString();


                
                string jsonString = wc.DownloadString(builder.Uri.AbsoluteUri);

                var jss = new JavaScriptSerializer();
                var returnData = jss.Deserialize<Dictionary<string, dynamic>>(jsonString);

                cat = returnData["results"]["release"]["catalogNumber"];

                _catalogNumberCache.Add(p, cat);
            
            }

            return cat;
        }

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

        private static DateTime GetReleaseDate(string dateStr)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^(\d{4})-(\d{2})-(\d{2})$");

            var m = regex.Match(dateStr);

            if(!m.Success) throw new InvalidOperationException("Error reading date string:"+dateStr);


            int year = System.Convert.ToInt32(m.Groups[1].Value);
            int month = System.Convert.ToInt32(m.Groups[2].Value);
            int day = System.Convert.ToInt32(m.Groups[3].Value);


            return new DateTime(year, month, day);
        }

        private static Key GetKey(dynamic key)
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

            Key returnKey = KeyEnumStringConverter.ConvertFromString(keyString);

            return returnKey;


        }

        private static string GetGenre(System.Collections.ArrayList genres)
        {
            List<string> genreNames = new List<string>();

            foreach(Dictionary<string,dynamic> genre in genres)
            {
                genreNames.Add((string)genre["name"]);
            }

            string genreStr = string.Join(", ", genreNames);

            return genreStr;
        }

        private Dictionary<String, dynamic> GetTrackData(string searchQuery, int page, int tracksPerPage)
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

                //updates total results
                _totalPages = returnDict["metadata"]["totalPages"];

            }

            return returnDict;
        }



        public bool HasMoreResults
        {
            get;
            private set;
        }
    }
}




