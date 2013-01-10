using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TracktorTagger
{
    public class BeatPortTrackDataSearch : ITrackDataSearch
    {
        int _totalResults;
        string _query;
        private int _trackPerPage;
        private int _currentPage;
        private int _totalPages;

        List<TrackData> _trackData;


        public BeatPortTrackDataSearch(BeatportTrackDataSource source, string query, int trackPerPage)
        {
            _query = query;
            _trackPerPage = trackPerPage;
            _currentPage = 1;
            _totalResults = 0;
            _trackData = new List<TrackData>();

            DataSource = source;

            LoadMoreResults();
        }








        public string SearchQuery
        {
            get { return _query; }
        }

        public int TotalResults
        {
            get { return _totalResults; }
        }

        public IList<TrackData> Results
        {
            get { return _trackData.AsReadOnly(); }
        }

        public IList<TrackData> LoadMoreResults()
        {
            //if all results are loaded just return an empty list
            if(_currentPage > _totalPages) return new List<TrackData>();

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

                
                               
                
                KeyEnum key = GetKey(result["key"]);

                
                

                TrackData track = new TrackData(DataSource.Name, trackId, artist, title, mix, remixer, release, producer, label, catalogNo, genre, key, releaseDate, url);

                newTracks.Add(track);
            }

            _currentPage++;

            _trackData.AddRange(newTracks);



            return newTracks.AsReadOnly();
        }

        Dictionary<int, string> _catalogNumberCache = new Dictionary<int, string>();

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

        private string GetArtist(System.Collections.ArrayList artists, string type)
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

        private Uri GetUri(int id, string slub)
        {
            UriBuilder builder = new UriBuilder("http://www.beatport.com");

            builder.Path = "track/" + slub + "/" + id.ToString();

            return builder.Uri;
        }

        private DateTime GetReleaseDate(string dateStr)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^(\d{4})-(\d{2})-(\d{2})$");

            var m = regex.Match(dateStr);

            if(!m.Success) throw new InvalidOperationException("Error reading date string:"+dateStr);


            int year = System.Convert.ToInt32(m.Groups[1].Value);
            int month = System.Convert.ToInt32(m.Groups[2].Value);
            int day = System.Convert.ToInt32(m.Groups[3].Value);


            return new DateTime(year, month, day);
        }

        private KeyEnum GetKey(dynamic key)
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

        private string GetGenre(System.Collections.ArrayList genres)
        {
            List<string> genreNames = new List<string>();

            foreach(Dictionary<string,dynamic> genre in genres)
            {
                genreNames.Add((string)genre["name"]);
            }

            string genreStr = string.Join(", ", genreNames);

            return genreStr;
        }



        public ITrackDataSource DataSource
        {
            get;
            private set;
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
                _totalResults = returnDict["metadata"]["count"];
                _totalPages = returnDict["metadata"]["totalPages"];

            }

            return returnDict;
        }

















        public IEnumerable<TrackData> SearchTracks(string searchQuery)
        {
            var trackJsonString = string.Empty;
            string providerName = this.GetType().FullName;



            if(string.IsNullOrEmpty(searchQuery)) throw new ArgumentNullException("searchQuery");


            List<TrackData> returnTracks = new List<TrackData>();


            try
            {

                using(var webclient = new WebClient())
                {


                    System.UriBuilder trackDataUrlBuilder = new UriBuilder("http:", "api.beatport.com");
                    trackDataUrlBuilder.Path = "catalog/3/search";


                    string query = "query=" + searchQuery + "&page=" + 1 + "&facets[]=fieldType:track&perPage=" + _trackPerPage.ToString();

                    trackDataUrlBuilder.Query = query;



                    trackJsonString = webclient.DownloadString(trackDataUrlBuilder.Uri.AbsoluteUri);


                    var jss = new JavaScriptSerializer();
                    var trackDataDict = jss.Deserialize<Dictionary<string, dynamic>>(trackJsonString);

                    int count = trackDataDict["metadata"]["count"];




                    foreach(Dictionary<string, Object> trackData in trackDataDict["results"])
                    {

                        //gets the release data



                        int releaseId = (int)(((Dictionary<string, object>)trackData["release"])["id"]);

                        System.UriBuilder releaseDataUriBuilder = new UriBuilder("http:", "api.beatport.com");
                        releaseDataUriBuilder.Path = "catalog/3/beatport/release";

                        string releaseQuery = "id=" + releaseId.ToString();
                        releaseDataUriBuilder.Query = releaseQuery;

                        string trackReleaseJsonString = webclient.DownloadString(releaseDataUriBuilder.Uri.AbsoluteUri);

                        var trackReleaseDataDict = jss.Deserialize<Dictionary<string, dynamic>>(trackReleaseJsonString);


                        Dictionary<string, object> releaseData = trackReleaseDataDict["results"]["release"];



                        //null parameters for track constructor
                        string TrackId = null;
                        string URL = null;

                        string Artist = null;
                        string Title = null;

                        string Remixer = null;
                        string Mix = null;

                        string Release = null;
                        string Producer = null;

                        string Label = null;
                        string CatalogNumber = null;

                        string Lyrics = null;

                        string Genre = null;
                        KeyEnum? Key = null;

                        DateTime? ReleaseDate = null;




                        //Title
                        string title = (string)trackData["name"];
                        if(!string.IsNullOrEmpty(title)) Title = title;



                        //Remixer(s)
                        System.Collections.ArrayList remixers = (System.Collections.ArrayList)trackData["artists"];

                        List<string> remixerNames = new List<string>();

                        foreach(Dictionary<string, object> remixer in remixers)
                        {
                            string remixerType = (string)remixer["type"];

                            if(remixerType != "remixer") continue;

                            remixerNames.Add((string)remixer["name"]);
                        }

                        string remixerStr = string.Join(", ", remixerNames);
                        if(!String.IsNullOrEmpty(remixerStr)) Remixer = remixerStr;




                        //Mix
                        string mix = (string)trackData["mixName"];
                        if(!string.IsNullOrEmpty(mix))
                        {
                            Mix = mix;
                        }


                        //release date
                        string releaseDateStr = (string)trackData["releaseDate"];
                        if(!string.IsNullOrEmpty(releaseDateStr)) ReleaseDate = DateTime.Parse(releaseDateStr);


                        //Label
                        Dictionary<string, object> labelDict = (Dictionary<string, object>)trackData["label"];
                        string labelStr = (string)labelDict["name"];
                        if(!string.IsNullOrEmpty(labelStr)) Label = labelStr;

                        //Album Title
                        Dictionary<string, object> releaseDict = (Dictionary<string, object>)trackData["release"];
                        string releaseStr = (string)releaseDict["name"];
                        if(!string.IsNullOrEmpty(releaseStr)) Release = releaseStr;









                        //track.CatalogNumber;

                        CatalogNumber = (string)releaseData["catalogNumber"];











                        //track.Key;

                        Dictionary<string, object> keyData = (Dictionary<string, object>)trackData["key"];

                        if(keyData != null)
                        {
                            object standardKeyDataObj = keyData["standard"];

                            Dictionary<string, object> standardKeyData = (Dictionary<string, object>)keyData["standard"];

                            string letterString = (string)standardKeyData["letter"];
                            char letter = letterString[0];

                            bool sharp = (bool)standardKeyData["sharp"];
                            bool flat = (bool)standardKeyData["flat"];



                            Key = KeyEnum.A_flat_minor;
                        }





                        //URL
                        System.UriBuilder trackUriBuilder = new UriBuilder("http:", "www.beatport.com");



                        string slugString = (string)trackData["slug"];
                        int trackId = (int)trackData["id"];
                        TrackId = trackId.ToString();




                        trackUriBuilder.Path = "track/" + slugString + "/" + TrackId;

                        URL = trackUriBuilder.Uri.AbsoluteUri;


                        //Not Support by BeatPort
                        //track.Lyrics;
                        //track.Producer;


                        ///gets the track id


                        TrackData track = new TrackData("beatport.com", TrackId, Artist, Title, Mix, Remixer, Release, Producer, Label, CatalogNumber, Genre, Key, ReleaseDate, new Uri(URL));

                        returnTracks.Add(track);

                    }


                }//close web client



            }
            catch
            {
                throw;
            }


            return returnTracks;



        }


    }
}




