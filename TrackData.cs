using System;
namespace TracktorTagger
{
    public class TrackData
    {
        
        public string TrackId { get; private set; }
        public Uri URL { get; private set; }

        public string Artist { get; private set; }
        public string Title { get; private set; }

        public string Remixer { get; private set; }
        public string Mix { get; private set; }

        public string Release { get; private set; }
        public string Producer { get; private set; }

        public string Label { get; private set; }
        public string CatalogNumber { get; private set; }

        public string Lyrics { get; private set; }

        public string Genre { get; private set; }
        public KeyEnum? Key { get; private set; }
       
        public DateTime? ReleaseDate { get; private set; }


        public TrackDataSourceTag DataSourceTag { get; private set; }


        public TrackData(string dataSourceName, string trackId, string artist, 
            string title, 
            string mix,
            string remixer, 
            string release, 
            string producer, 
            string label, 
            string catalogNo, 
            string lyrics, 
            string genre, 
            KeyEnum? key, 
            DateTime? releaseDate, 
            Uri url)
        {
            this.TrackId = trackId;


            this.Artist = artist;
            this.Title = title;
            this.Remixer = remixer;
            this.Mix = mix;
            this.Release = release;
            this.Producer = producer;
            this.Label = label;
            this.CatalogNumber = catalogNo;
            this.Lyrics = lyrics;
            this.Genre = genre;
            this.Key = key;
            this.ReleaseDate = releaseDate;
            this.URL = url;

            
            DataSourceTag = new TrackDataSourceTag(dataSourceName, trackId, url);
        }

    }
}
