using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Net;

namespace TracktorTagger
{
    public class DiscogsTrackDataSource : ITrackDataSource
    {
        int _perPage;
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

        public DiscogsTrackDataSource()
        {
            PerPage = 10;
        }


      

        public TrackData GetTrack(string trackId)
        {
            throw new NotImplementedException();
        }


        public int GetTotalResultCount(string searchQuery)
        {
            throw new NotImplementedException();
        }


        public string Name
        {
            get { return "discogs.com"; }
        }


        ITrackDataSearch ITrackDataSource.GetTrackDataSearch(string searchQuery)
        {
            return new DiscogsTrackDataSearch(this, searchQuery, PerPage);
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
    }
}


