using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web.Script.Serialization;

namespace TracktorTagger
{
    public class BeatPortTrackDataProvider : ITrackDataProvider
    {
        public IEnumerable<Track> GetTracks(string searchQuery)
        {
            var json_data = string.Empty;

            if (string.IsNullOrEmpty(searchQuery)) throw new ArgumentNullException("searchQuery");


            try
            {

                using (var webclient = new WebClient())
                {


                    System.UriBuilder uriBuilder = new UriBuilder("http:", "api.beatport.com");
                    uriBuilder.Path = "catalog/2/search";

                    string query = "query=" + searchQuery + "&page=" + 2 + "&facets[]=fieldType:track";

                    uriBuilder.Query = query;



                    





                    

                        


                    json_data = webclient.DownloadString(uriBuilder.Uri.AbsoluteUri);


                    var jss = new JavaScriptSerializer();
                    var dict = jss.Deserialize<Dictionary<string,dynamic>>(json_data);



                }


            }
            catch
            {

                throw;

            }






            return new List<Track>();




        }
    }
}
