using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TracktorTagger
{
    public class DiscogsTrackDataSearch:ITrackDataSearch
    {

        string _query;
        int _perPage;
        int _totalPages;
        int _currentPage;

        private List<TrackData> _results = new List<TrackData>();



        public DiscogsTrackDataSearch(DiscogsTrackDataSource dataSource, string query, int tracksPerPage)
        {
            DataSource = dataSource;
            _query = query;
            _perPage = tracksPerPage;


            _currentPage = 1;


            LoadMoreResults();            
        }


        public string SearchQuery
        {
            get { return _query; }
        }

        public int TotalResults
        {
            get;
            private set;
        }

        public IList<TrackData> Results
        {
            get { return _results.AsReadOnly(); }
        }

        public IList<TrackData> LoadMoreResults()
        {
            //if all results are loaded just return an empty list
            

            var data = GetTrackData(_query,_currentPage,_perPage);


            List<TrackData> newTracks = new List<TrackData>();


            foreach(Dictionary<string, dynamic> result in data["results"])
            {

                string trackId = "id";
                string artist = "art";
                    string title = "tit";
                string mix = "mix";
                string remixer = "remix";
                string release = "release";
                string producer = "prod";
                string label = "labl";
                string catalogNo = "cat";
                string genre = "genres";
                DateTime releaseDate = DateTime.Now;
                Uri url = new Uri("http://www.google.com");



                TrackData newTrack = new TrackData(DataSource.Name, trackId, artist, title, mix, remixer, release, producer, label, catalogNo, genre, null, releaseDate, url);
                newTracks.Add(newTrack);

            }




            _currentPage++;

            _results.AddRange(newTracks);

            return newTracks.AsReadOnly();

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
                System.UriBuilder trackDataUrlBuilder = new UriBuilder("http:", "api.discogs.com");
                trackDataUrlBuilder.Path = "database/search";


                //discogs does not have information about induvidual tracks
                string query = "q=" + searchQuery + "&page=" + page.ToString() + "&type=release&per_page=" + tracksPerPage.ToString();
                trackDataUrlBuilder.Query = query;

                string jsonString = webclient.DownloadString(trackDataUrlBuilder.Uri.AbsoluteUri);

                var jss = new JavaScriptSerializer();
                returnDict = jss.Deserialize<Dictionary<string, dynamic>>(jsonString);

                //updates total results
                TotalResults = returnDict["pagination"]["items"];
                _totalPages = returnDict["pagination"]["pages"];
            }

            return returnDict;
        }





        public IEnumerable<TrackData> SearchTracks(string searchQuery)
        {

            string providerName = this.GetType().FullName;

            if(string.IsNullOrEmpty(searchQuery)) throw new ArgumentNullException("searchQuery");


            List<TrackData> returnTracks = new List<TrackData>();


            var jss = new JavaScriptSerializer();

            try
            {

                using(var webclient = new WebClient())
                {


                    System.UriBuilder trackDataUrlBuilder = new UriBuilder("http:", "api.discogs.com");
                    trackDataUrlBuilder.Path = "database/search";



                    string query = "q=" + searchQuery + "&page=" + 1 + "&type=release&per_page=" + this._perPage .ToString();

                    trackDataUrlBuilder.Query = query;



                    string releaseSearchResultsString = webclient.DownloadString(trackDataUrlBuilder.Uri.AbsoluteUri);



                    var releaseSearchResultsDict = jss.Deserialize<Dictionary<string, dynamic>>(releaseSearchResultsString);

                    var results = releaseSearchResultsDict["results"];

                    foreach(Dictionary<string, object> releaseSearchResults in results)
                    {

                        var releaseDataUrl = (string)releaseSearchResults["resource_url"];


                        string releaseResultsString = webclient.DownloadString(new Uri(releaseDataUrl));


                        var releaseData = jss.Deserialize<Dictionary<string, dynamic>>(releaseResultsString);


                        var formats = releaseData["formats"];

                        bool isVinyl = false;

                        foreach(Dictionary<string, object> format in formats)
                        {
                            var formatString = (string)format["name"];
                            if(formatString == "Vinyl") isVinyl = true;
                        }
                        if(!isVinyl) continue;




                        //url
                        string URL = (string)releaseData["uri"];

                        //release
                        string Release = (string)releaseData["title"];


                        //Label Catalog Num

                        string Label = null;
                        string CatalogNumber = null;

                        System.Collections.ArrayList labels = releaseData["labels"];

                        if(labels.Count > 0)
                        {
                            Dictionary<string, object> labelDict = (Dictionary<string, object>)labels[0];

                            Label = (string)labelDict["name"];
                            CatalogNumber = (string)labelDict["catno"];
                        }


                        //Genre(s)

                        string Genre = null;

                        if(releaseData.ContainsKey("styles"))
                        {
                            System.Collections.ArrayList genres = (System.Collections.ArrayList)releaseData["styles"];

                            List<string> genreNames = new List<string>();

                            foreach(String genre in genres)
                            {
                                genreNames.Add(genre);
                            }

                            string genreStr = string.Join(", ", genreNames);
                            if(!String.IsNullOrEmpty(genreStr)) Genre = genreStr;
                        }


                        //Release date
                        DateTime? ReleaseDate = null;

                        if(releaseData.ContainsKey("released"))
                        {
                            string releasedString = (string)releaseData["released"];

                            var yearMatch = System.Text.RegularExpressions.Regex.Match(releasedString, @"\d{4}$");
                            var dateMatch = System.Text.RegularExpressions.Regex.Match(releasedString, @"(\d{4})-(\d{2})-(\d{2})$");

                            if(yearMatch.Success)
                            {
                                ReleaseDate = new DateTime(System.Convert.ToInt32(releasedString), 1, 1);
                            }
                            else if(dateMatch.Success)
                            {

                                int year = System.Convert.ToInt32(dateMatch.Groups[1].Value);
                                int month = System.Convert.ToInt32(dateMatch.Groups[2].Value);
                                int day = System.Convert.ToInt32(dateMatch.Groups[3].Value);

                                if(month == 0 || day == 0) new DateTime(year, 1, 1);
                                else ReleaseDate = new DateTime(year, month, day);


                            }
                            else
                            {
                                throw new InvalidOperationException("Unexpected release date format:" + releasedString);

                            }
                        }






                        //artists

                        List<string> releaseArtists = new List<string>();
                        List<string> releaseRemixers = new List<string>();
                        List<string> releaseProducers = new List<string>();


                        foreach(Dictionary<string, object> artist in releaseData["artists"])
                        {
                            var name = (string)artist["name"];
                            var anv = (string)artist["anv"];
                            var role = (string)artist["role"];

                            if(name == "Various") continue;

                            if(!string.IsNullOrEmpty(anv)) name = anv;



                            if(!string.IsNullOrEmpty(role))
                            {
                                throw new InvalidOperationException("found release artist with a roll: " + role);
                            }
                            else
                            {
                                releaseArtists.Add(name);
                            }


                        }

                        //if (releaseData.ContainsKey("extraartists"))
                        //{

                        //    foreach (Dictionary<string, object> artist in releaseData["extraartists"])
                        //    {
                        //        var name = (string)artist["name"];
                        //        var anv = (string)artist["anv"];
                        //        var role = (string)artist["role"];

                        //        if (name == "Various") continue;

                        //        if (!string.IsNullOrEmpty(anv)) name = anv;


                        //        if (!string.IsNullOrEmpty(role))
                        //        {
                        //            releaseArtists.Add(name);
                        //        }
                        //        else if (role == "Remix")
                        //        {
                        //            releaseRemixers.Add(name);
                        //        }
                        //        else if (role.Contains("Producer"))
                        //        {
                        //            releaseProducers.Add(name);
                        //        }
                        //        else
                        //        {
                        //            throw new InvalidOperationException("found release extra artist with an unknown roll: " + role);
                        //        }


                        //    }
                        //}

                        foreach(Dictionary<string, object> trackData in releaseData["tracklist"])
                        {


                            int trackIdInt = (int)releaseData["id"];
                            string TrackId = trackIdInt.ToString();


                            string Title = null;
                            string Mix = null;

                            string titleString = (string)trackData["title"];


                            System.Text.RegularExpressions.Regex trackMixRegex = new System.Text.RegularExpressions.Regex(@"^(.*)\((.*)\)");



                            var trackMixMatch = trackMixRegex.Match(titleString);

                            if(trackMixMatch.Success)
                            {
                                Title = trackMixMatch.Groups[1].Value;
                                Mix = trackMixMatch.Groups[2].Value;
                            }
                            else
                            {
                                Title = titleString;
                            }





                            //track artists


                            List<string> trackArtists = new List<string>();
                            List<string> trackRemixers = new List<string>();
                            List<string> trackProducers = new List<string>();



                            if(trackData.ContainsKey("artists"))
                            {

                                System.Collections.ArrayList trackArtistsList = (System.Collections.ArrayList)trackData["artists"];

                                foreach(Dictionary<string, object> artist in trackArtistsList)
                                {
                                    var name = (string)artist["name"];
                                    var anv = (string)artist["anv"];
                                    var role = (string)artist["role"];

                                    if(!string.IsNullOrEmpty(anv)) name = anv;

                                    if(string.IsNullOrEmpty(role))
                                    {
                                        trackArtists.Add(name);
                                    }
                                    else if(role == "Remix")
                                    {
                                        trackRemixers.Add(name);
                                    }
                                    else
                                    {
                                        throw new InvalidOperationException("found track artist with an unknown roll: " + role);
                                    }
                                }

                            }


                            if(trackData.ContainsKey("extraartists"))
                            {

                                System.Collections.ArrayList trackextraArtists = (System.Collections.ArrayList)trackData["extraartists"];

                                foreach(Dictionary<string, object> artist in trackextraArtists)
                                {
                                    var name = (string)artist["name"];
                                    var anv = (string)artist["anv"];
                                    var role = (string)artist["role"];

                                    if(!string.IsNullOrEmpty(anv)) name = anv;

                                    if(string.IsNullOrEmpty(role))
                                    {
                                        trackArtists.Add(name);
                                    }
                                    else if(role.Contains("Remix"))
                                    {
                                        trackRemixers.Add(name);
                                    }
                                }

                            }


                            string Artist = null;
                            var artistList = releaseArtists.Union(trackArtists);
                            string artistStr = string.Join(", ", artistList);
                            if(!String.IsNullOrEmpty(artistStr)) Artist = artistStr;

                            string Remixer = null;
                            var remixerList = releaseRemixers.Union(trackRemixers);
                            string remixerStr = string.Join(", ", remixerList);
                            if(!String.IsNullOrEmpty(remixerStr)) Remixer = remixerStr;


                            string Producer = null;
                            var producerList = releaseProducers.Union(trackProducers);
                            string producerStr = string.Join(", ", producerList);
                            if(!String.IsNullOrEmpty(producerStr)) Producer = producerStr;


                            TrackData track = new TrackData("discogs.com", TrackId, Artist, Title, Mix, Remixer, Release, Producer, Label, CatalogNumber, Genre, null, ReleaseDate, new Uri(URL));

                            returnTracks.Add(track);
                        }

                    }

                }
            }
            catch
            {

            }


            return returnTracks;
        }


    }
}

