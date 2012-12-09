using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TracktorTagger
{
    public class TrackDataSourceTag
    {

        public TrackDataSourceTag(string trackDataProvider, string trackId, Uri trackDataUrl)
        {
            if (string.IsNullOrEmpty(trackDataProvider)) throw new ArgumentNullException("trackDataProvder");
            if (string.IsNullOrEmpty(trackId)) throw new ArgumentNullException("trackId");
            if (trackDataUrl == null) throw new ArgumentNullException("trackDataUrl");

            this.TrackId = trackId;
            this.TrackDataProvider = trackDataProvider;
            this.TrackDataUrl = trackDataUrl;
        }

        /// <summary>
        /// 
        /// </summary>
        public string TrackId
        {
            get;
            private set;
        }

        /// <summary>
        /// Full Class Name of the TrackDataProvider
        /// </summary>
        public string TrackDataProvider
        {
            get;
            private set;
        }

        public Uri TrackDataUrl
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return TrackDataProvider;
        }


    }
}
