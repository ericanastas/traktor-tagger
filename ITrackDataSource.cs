using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorTagger
{
    public interface ITrackDataSource
    {

        /// <summary>
        /// Host URL of datasource
        /// </summary>
        /// <remarks>This is used to identify the datasource</remarks>
        string HostName { get; }

        ITrackDataSearch GetTrackDataSearch(string searchQuery);

        ITrackDataSearch GetTrackDataSearch(Uri searchUri);
        

        TrackData GetTrack(string trackId);



        /// <summary>
        /// Short message which identifies the format(s) of search queries for the data source
        /// </summary>
        String SearchPrompt { get; }


        bool ProvidesTitle { get; }
        bool ProvidesMix { get; }
        bool ProvidesArtist { get; }
        bool ProvidesRemixer { get; }
        bool ProvidesProducer { get; }
        bool ProvidesRelease { get; }
        bool ProvidesReleaseDate { get; }
        bool ProvidesLabel { get; }
        bool ProvidesCatalogNo { get; }
        bool ProvidesGenre { get; }
        bool ProvidesKey { get; }

    }


}
