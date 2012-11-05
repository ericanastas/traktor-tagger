using System;
namespace TracktorTagger
{
    public class Track
    {
        string Artist { get; set; }
        string Title { get; set; }

        string Remixer { get; set; }
        string Mix { get; set; }

        string AlbumTitle { get; set; }
        string Producer { get; set; }





        string Label { get; set; }
        string CatalogNumber { get; set; }



        string Comment1 { get; set; }
        string Comment2 { get; set; }
        string Lyrics { get; set; }

        string Genre { get; set; }
        string Key { get; set; }
       
        DateTime? ReleaseDate { get; set; }
 
    }
}
