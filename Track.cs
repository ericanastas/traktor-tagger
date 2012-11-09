using System;
namespace TracktorTagger
{
    public class Track
    {
        public string Artist { get; set; }
        public string Title { get; set; }

        public string Remixer { get; set; }
        public string Mix { get; set; }

        public string Release { get; set; }
        public string Producer { get; set; }

        public string Label { get; set; }
        public string CatalogNumber { get; set; }

        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string Lyrics { get; set; }

        public string Genre { get; set; }
        public Key Key { get; set; }
       
        public DateTime? ReleaseDate { get; set; }

        public string URL { get; set; }

    }
}
