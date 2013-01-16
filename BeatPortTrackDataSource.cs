using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web.Script.Serialization;

namespace TraktorTagger
{
    public class BeatportTrackDataSource : ITrackDataSource
    {

        public BeatportTrackDataSource(int perPage)
        {
            TrackPerPage = perPage;
        }

        int _perPage;


        public int TrackPerPage
        {
            get
            { return _perPage; }
            set
            {
                if(value > 0 & value <= 150)
                {
                    _perPage = value;
                }
            }
        }


        public TrackData GetTrack(string trackId)
        {
            throw new NotImplementedException();
        }


        public string Name
        {
            get { return "www.beatport.com"; }
        }

        public ITrackDataSearch GetTrackDataSearch(string searchQuery)
        {
            BeatPortTrackDataSearch search = new BeatPortTrackDataSearch(this,searchQuery,this.TrackPerPage);

            return search;
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
            get { return false; }
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
            get { return true; }
        }


        public ITrackDataSearch GetTrackDataSearch(Uri searchUri)
        {
            throw new NotImplementedException();
        }
    }
}
