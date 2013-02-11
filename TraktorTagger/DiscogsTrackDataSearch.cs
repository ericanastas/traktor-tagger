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

        int _perPage;
        int _totalPages;
        int _nextPage;
        string _format;

        public bool HasMoreResults
        {
            get;
            private set;
        }

        public ITrackDataSource Source
        {
            get;
            private set;
        }

        private List<TrackData> _results;


        IList<string> _remixRoles;
        IList<string> _producerRoles;

        public DiscogsTrackDataSearch(DiscogsTrackDataSource source, Uri releaseUri)
        {
            if(source == null) throw new ArgumentNullException("source");
            if(releaseUri == null) throw new ArgumentNullException("releaseUri");

            this.SearchQuery = releaseUri.AbsoluteUri;
            this.HasMoreResults = false;
            this.Source = source;

            //Beatport.com URL regex
            System.Text.RegularExpressions.Regex releaseURIRegex = new System.Text.RegularExpressions.Regex(@"^http://www.discogs.com/(.*)/release/(\d*)$");


            var uriMatch = releaseURIRegex.Match(releaseUri.AbsoluteUri);

            if(uriMatch.Success)
            {
                int releaseId = System.Convert.ToInt32(uriMatch.Groups[2].Value);


                Dictionary<string, dynamic> releaseData = GetReleaseData(releaseId);

                IEnumerable<TrackData> releaseTracks = ParseReleaseData(releaseData,this.Source.Host);

                _results = new List<TrackData>(releaseTracks);
            }
            else
            {
                throw new ArgumentException("Invalid Discogs release URI format", "searchUri");
            }
        }

        public DiscogsTrackDataSearch(DiscogsTrackDataSource dataSource, string query, int tracksPerPage, string format)
        {
            HasMoreResults = true;

            Source = dataSource;
            SearchQuery = query;
            _perPage = tracksPerPage;

            _nextPage = 1;
            _format = format;


            _results = new List<TrackData>();


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

            if(!HasMoreResults) return new List<TrackData>();


            var data = GetSearchResultsData(SearchQuery, _nextPage, _perPage,this._format);

            _totalPages = data["pagination"]["pages"];

            List<TrackData> newTracks = new List<TrackData>();

            foreach(Dictionary<string, dynamic> result in data["results"])
            {

                int releaseId = result["id"];

                var releaseData = GetReleaseData(releaseId);

                var tracks = ParseReleaseData(releaseData, this.Source.Host);
                
                newTracks.AddRange(tracks);
            }

            if(_nextPage == _totalPages)
            {
                HasMoreResults = false;
            }
            else
            {
                _nextPage++;
            }

            _results.AddRange(newTracks);

            return newTracks.AsReadOnly();

        }

        #region Parsing Methods



        private static IEnumerable<TrackData> ParseReleaseData(Dictionary<string, dynamic> releaseData, string host)
        {
            //url
            string URL = (string)releaseData["uri"];

            //release
            string Release = (string)releaseData["title"];

            

            //Label and catalog number
            System.Collections.ArrayList labels = releaseData["labels"];

            List<string> labelNames = new List<string>();
            List<string> catNumbers = new List<string>();

            foreach(Dictionary<string,dynamic> labelData in labels)
            {
                labelNames.Add(labelData["name"]);
                catNumbers.Add(labelData["catno"]);
            }

            string Label = string.Join(", ", labelNames);
            string CatalogNumber = string.Join(", ", catNumbers);

            


            

            


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


                TrackData track = new TrackData(host, TrackId, Artist, Title, Mix, Remixer, Release, Producer, Label, CatalogNumber, Genre, null, ReleaseDate, new Uri(URL));

                yield return track;
            }
        }




        private static IEnumerable<string> GetArtists(dynamic artistsData, IList<string> allowableRoles)
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


        #endregion

        #region Data Retrival Methods


        private static Dictionary<String, dynamic> GetSearchResultsData(string searchQuery, int page, int tracksPerPage, string format)
        {
            Dictionary<String, dynamic> returnDict;

            using(var webclient = new WebClient())
            {
                System.UriBuilder trackDataUrlBuilder = new UriBuilder("http:", "api.discogs.com");
                trackDataUrlBuilder.Path = "database/search";

                //discogs does not have information about induvidual tracks
                string query = "q=" + searchQuery + "&page=" + page.ToString() + "&type=release&per_page=" + tracksPerPage.ToString();

                if(!String.IsNullOrEmpty(format))
                {
                    query = query + "&format=" + format;
                }

                trackDataUrlBuilder.Query = query;

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
                System.UriBuilder trackDataUrlBuilder = new UriBuilder("http:", "api.discogs.com");
                trackDataUrlBuilder.Path = "releases/" + releaseId.ToString();

                string jsonString = webclient.DownloadString(trackDataUrlBuilder.Uri.AbsoluteUri);

                var jss = new JavaScriptSerializer();
                returnDict = jss.Deserialize<Dictionary<string, dynamic>>(jsonString);
            }

            return returnDict;
        }

        #endregion



    }
}







