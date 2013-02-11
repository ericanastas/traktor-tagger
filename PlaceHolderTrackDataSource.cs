using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorTagger
{



    public class PlaceHolderTrackDataSource : ITrackDataSource
    {

        public PlaceHolderTrackDataSource()
        {
        
        }


        public TrackData GetTrack(string trackId)
        {
            TrackData data = new TrackData("PlaceHolderTrackDataSource",
                "id" + trackId,
                "artist " + trackId,
                "title " + trackId,
                "mix " + trackId,
                "remixer " + trackId,
                "release " + trackId,
                "producer " + trackId,
                "label " + trackId,
                "cat no " + trackId,
                "genre " + trackId,
                 KeyEnum.A,
                 DateTime.Now,
                 new Uri(@"http://www.google.com"));

            return data;
        }

        public string Host
        {
            get { return "traktortaggerplacholder.com"; }
        }

        public ITrackDataSearch GetTrackDataSearch(string searchQuery)
        {
            var s = new PlaceHolderTrackDataSearch(this, searchQuery, 100, 10);
            
            return s;
        }

        public ITrackDataSearch GetTrackDataSearch(Uri searchUri)
        {
            var s = new PlaceHolderTrackDataSearch(this, searchUri.ToString(), 2, 2);

            return s;
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
            get { return true; }
        }




    }
}
