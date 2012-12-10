using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TracktorTagger
{
    public class TrackDataSourceTag
    {

        public TrackDataSourceTag(string trackDataSourceName, string trackId, Uri trackDataUrl)
        {
            if (string.IsNullOrEmpty(trackDataSourceName)) throw new ArgumentNullException("trackDataProvder");
            if (string.IsNullOrEmpty(trackId)) throw new ArgumentNullException("trackId");
            if (trackDataUrl == null) throw new ArgumentNullException("trackDataUrl");

            this.TrackId = trackId;
            this.DataSource = trackDataSourceName;
            this.Uri = trackDataUrl;
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
        public string DataSource
        {
            get;
            private set;
        }

        public Uri Uri
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return DataSource + ": " + TrackId;
        }

    }
}
