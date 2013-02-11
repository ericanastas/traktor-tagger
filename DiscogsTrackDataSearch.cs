using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TraktorTagger
{
    public class DiscogsTrackDataSearch : ITrackDataSearch
    {

        string _query;
        int _perPage;
        int _totalPages;
        int _currentPage;
        string _format;


        private List<TrackData> _results = new List<TrackData>();

        
        IList<string> _remixRoles;
        IList<string> _producerRoles;


        public DiscogsTrackDataSearch(DiscogsTrackDataSource dataSource, Uri discogsTrackUrl)
        {
            this._query = discogsTrackUrl.AbsoluteUri;
            this.HasMoreResults = false;
            this.DataSource = dataSource;

            throw new NotImplementedException();






        }



        public DiscogsTrackDataSearch(DiscogsTrackDataSource dataSource, string query, int tracksPerPage, string format)
        {
            HasMoreResults = true;

            DataSource = dataSource;
            _query = query;
            _perPage = tracksPerPage;

            _currentPage = 1;
            _format = format;


            InitRoles();



            LoadMoreResults();
        }

        private void InitRoles()
        {
            _remixRoles = new List<string>();
            _remixRoles.Add("Remix");
            _remixRoles.Add("Remixer");
            _remixRoles.Add("remix");
            _remixRoles.Add("remixer");


            _producerRoles = new List<string>();
            _producerRoles.Add("Producer");
            _producerRoles.Add("producer");

        }

        public string SearchQuery
        {
            get { return _query; }
        }

        public IList<TrackData> Results
        {
            get { return _results.AsReadOnly(); }
        }

        public IList<TrackData> LoadMoreResults()
        {
            //if all results are loaded just return an empty list

            if(!HasMoreResults) return new List<TrackData>();


            var data = GetTrackData(_query, _currentPage, _perPage);


            List<TrackData> newTracks = new List<TrackData>();


            foreach(Dictionary<string, dynamic> result in data["results"])
            {
                var tracks = GetResultsTracks(result);

                newTracks.AddRange(tracks);
            }


            if(_currentPage == _totalPages)
            {

                HasMoreResults = false;
            }
            else
            {
                _currentPage++;
            }

            _results.AddRange(newTracks);

            return newTracks.AsReadOnly();

        }





        private IList<TrackData> GetResultsTracks(Dictionary<string, dynamic> releaseSearchResult)
        {
            var returnTracks = new List<TrackData>();



            using(var webclient = new WebClient())
            {

                string releaseUrl = releaseSearchResult["resource_url"];
                Uri releaseDataUri = new Uri(releaseUrl);


                string jsonString = webclient.DownloadString(releaseDataUri.AbsoluteUri);
                var jss = new JavaScriptSerializer();
                Dictionary<string, dynamic> releaseData = jss.Deserialize<Dictionary<string, dynamic>>(jsonString);


                //release title
                string release = releaseData["title"];

                //get the url
                string urlString = releaseData["uri"];
                Uri url = new Uri(urlString);

                string label = null;
                string genre = null;
                string catalogNo = null;
                DateTime? releaseDate = null;



                if(releaseData.ContainsKey("labels"))
                {
                    label = GetLabel(releaseData["labels"]);
                    catalogNo = GetCatalogNo(releaseData["labels"]);
                }

                if(releaseData.ContainsKey("styles"))
                {
                    genre = GetGenres(releaseData["styles"]);
                }

                if(releaseData.ContainsKey("released"))
                {
                    releaseDate = GetReleaseDate(releaseData["released"]);
                }







                List<string> releaseArtists = new List<string>();
                List<string> releaseRemixers = new List<string>();
                List<string> releaseProducers = new List<string>();


                if(releaseData.ContainsKey("artists"))
                {
                    releaseArtists.AddRange(GetArtists(releaseData["artists"], null));
                    releaseRemixers.AddRange(GetArtists(releaseData["artists"], _remixRoles));
                    releaseProducers.AddRange(GetArtists(releaseData["artists"], _producerRoles));
                }

                if(releaseData.ContainsKey("extraartists"))
                {
                    releaseArtists.AddRange(GetArtists(releaseData["extraartists"], null));
                    releaseRemixers.AddRange(GetArtists(releaseData["extraartists"], _remixRoles));
                    releaseProducers.AddRange(GetArtists(releaseData["extraartists"], _producerRoles));
                }


                int trackNumber = 0;

                foreach(dynamic trackData in releaseData["tracklist"])
                {
                    //gets the trackId
                    //appends the number of the track to the releaseId
                    trackNumber++;
                    int trackIdint = releaseData["id"];
                    string trackId = trackIdint.ToString() + "_" + trackNumber.ToString();




                    //Title and Mix
                    string title;
                    string mix;

                    string titleString = trackData["title"];
                    var titleMixRegex = new System.Text.RegularExpressions.Regex(@"^(.*)\((.*)\)");
                    var titleMixMatch = titleMixRegex.Match(titleString);
                   
                    if(titleMixMatch.Success)
                    {
                        title = titleMixMatch.Groups[1].Value;
                        mix = titleMixMatch.Groups[2].Value;
                    }
                    else
                    {
                        title = titleString;
                        mix = null;
                    }


                    //Artist(s), remixer(s), and producer(s)

                    List<string> trackArtists = new List<string>();
                    List<string> trackRemixers = new List<string>();
                    List<string> trackProducers = new List<string>();

                    if(trackData.ContainsKey("artists"))
                    {
                        trackArtists.AddRange(GetArtists(trackData["artists"], null));
                        trackRemixers.AddRange(GetArtists(trackData["artists"], _remixRoles));
                        trackProducers.AddRange(GetArtists(trackData["artists"], _producerRoles));
                    }

                    if(trackData.ContainsKey("extraartists"))
                    {
                        trackArtists.AddRange(GetArtists(trackData["extraartists"], null));
                        trackRemixers.AddRange(GetArtists(trackData["extraartists"], _remixRoles));
                        trackProducers.AddRange(GetArtists(trackData["extraartists"], _producerRoles));
                    }


                    var fullArtistList = trackArtists.Union(releaseArtists).Distinct();
                    var fullRemixerList = trackRemixers.Union(releaseRemixers).Distinct();
                    var fullProducerList = trackProducers.Union(releaseProducers).Distinct();


                    string artist = null;
                    string remixer = null;
                    string producer = null;

                    if(fullArtistList.Count() > 0)
                    {
                        artist = String.Join(", ", fullArtistList);
                    }

                    if(fullRemixerList.Count() > 0)
                    {
                        remixer = String.Join(", ", fullRemixerList);
                    }

                    if(fullProducerList.Count() > 0)
                    {
                        producer = String.Join(", ", fullProducerList);
                    }

                    TrackData newTrack = new TrackData(DataSource.Host, trackId, artist, title, mix, remixer, release, producer, label, catalogNo, genre, null, releaseDate, url);

                    returnTracks.Add(newTrack);
                }
            }

            return returnTracks;


        }

        private IEnumerable<string> GetArtists(dynamic artistsData, IList<string> allowableRoles)
        {
            foreach(Dictionary<string, dynamic> artistData in artistsData)
            {
                string name = artistData["name"];
                string role = artistData["role"];
                string anv = artistData["anv"];


                string returnName;

                if(anv == String.Empty)
                {
                    returnName = name;
                }
                else
                {
                    returnName = anv;
                }

                var numberedNameMatch = System.Text.RegularExpressions.Regex.Match(name, @"^(.*)\s\((\d*)\)$");


                if(numberedNameMatch.Success)
                {
                    returnName = numberedNameMatch.Groups[1].Value;
                }

                if(name == "Various") continue; //skip Various

                
                if(allowableRoles != null)
                {
                    bool matchesRoles = false;

                    foreach(string r in allowableRoles)
                    {
                        if(role.Contains(r))
                        {
                            matchesRoles = true;
                            break;
                        }
                    }

                    if(matchesRoles)
                    {
                        yield return returnName;
                    }
                    else
                    {
                        continue;
                    }
                }
                else if(role == string.Empty)
                {
                    yield return returnName;
                }
                else
                {
                    continue;
                }

            }


        }

        private static DateTime? GetReleaseDate(string dateString)
        {
            if(dateString == null) return null;



            System.Text.RegularExpressions.Regex fullDateRegex = new System.Text.RegularExpressions.Regex(@"^(\d{4})-(\d{2})-(\d{2})$");
            System.Text.RegularExpressions.Regex yearDateRegex = new System.Text.RegularExpressions.Regex(@"^\d{4}$");


            var fullDateMatch = fullDateRegex.Match(dateString);
            var yearMatch = yearDateRegex.Match(dateString);


            if(fullDateMatch.Success)
            {
                int year = System.Convert.ToInt32(fullDateMatch.Groups[1].Value);
                int month = System.Convert.ToInt32(fullDateMatch.Groups[2].Value);
                int day = System.Convert.ToInt32(fullDateMatch.Groups[3].Value);

                if(month == 0) month = 1;
                if(day == 0) day = 1;

                return new DateTime(year, month, day);
            }
            else if(yearMatch.Success)
            {
                int year = System.Convert.ToInt32(dateString);

                return new DateTime(year, 1, 1);
            }
            else
            {
                throw new InvalidOperationException("Could not parse date: " + dateString);
            }

        }

        private static string GetGenres(System.Collections.ArrayList genres)
        {
            return string.Join(", ", genres.ToArray());
        }

        private static string GetCatalogNo(System.Collections.ArrayList labels)
        {
            List<string> labelList = new List<string>();

            foreach(dynamic labelData in labels)
            {
                labelList.Add((string)labelData["catno"]);
            }

            return string.Join(", ", labelList.ToArray());
        }

        private static string GetLabel(System.Collections.ArrayList labels)
        {
            List<string> labelList = new List<string>();

            foreach(dynamic labelData in labels)
            {

                string labelName = (string)labelData["name"];


                var numberedNameMatch = System.Text.RegularExpressions.Regex.Match(labelName, @"^(.*)\s\((\d*)\)$");


                if(numberedNameMatch.Success)
                {
                    labelName = numberedNameMatch.Groups[1].Value;
                }

                labelList.Add(labelName);
            }

            return string.Join(", ", labelList.ToArray());
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

                if(!String.IsNullOrEmpty(_format))
                {
                    query = query + "&format=" + _format;
                }

                trackDataUrlBuilder.Query = query;

                string jsonString = webclient.DownloadString(trackDataUrlBuilder.Uri.AbsoluteUri);

                var jss = new JavaScriptSerializer();
                returnDict = jss.Deserialize<Dictionary<string, dynamic>>(jsonString);

                //updates total results
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



                    string query = "q=" + searchQuery + "&page=" + 1 + "&type=release&per_page=" + this._perPage.ToString();

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

        public bool HasMoreResults
        {
            get;
            private set;
        }
    }
}







