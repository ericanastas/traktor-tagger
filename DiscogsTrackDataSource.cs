using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Net;

namespace TraktorTagger
{
    public class DiscogsTrackDataSource : ITrackDataSource
    {
        int _perPage;


        public string FormatFilter { get; private set; }


        public int PerPage
        {
            get
            { return _perPage; }
            set
            {
                if (value > 0 & value <= 100)
                {
                    _perPage = value;
                }
            }
        }

        public DiscogsTrackDataSource(int perPage, string formatFilter)
        {
            PerPage = perPage;
            FormatFilter = formatFilter;
        }



        public TrackData GetTrack(string trackId)
        {
            throw new NotImplementedException();
        }



        public string HostName
        {
            get { return "www.discogs.com"; }
        }


        public ITrackDataSearch GetTrackDataSearch(string searchQuery)
        {
            return new DiscogsTrackDataSearch(this, searchQuery, PerPage, FormatFilter);
        }


        public bool ProvidesTitle
        {
            get { return true; }
        }

        public bool ProvidesMix
        {
            get { return true; }
        }

        public bool ProvidesArtist
        {
            get { return true; }
        }

        public bool ProvidesRemixer
        {
            get { return true; }
        }

        public bool ProvidesProducer
        {
            get { return true; }
        }

        public bool ProvidesRelease
        {
            get { return true; }
        }

        public bool ProvidesReleaseDate
        {
            get { return true; }
        }

        public bool ProvidesLabel
        {
            get { return true; }
        }

        public bool ProvidesCatalogNo
        {
            get { return true; }
        }

        public bool ProvidesGenre
        {
            get { return true; }
        }

        public bool ProvidesKey
        {
            get { return false; }
        }


        public ITrackDataSearch GetTrackDataSearch(Uri searchUri)
        {
            throw new NotImplementedException();
        }


        public string SearchPrompt
        {
            get { return "Search query, or discogs.com release URL"; }
        }
    }
}


