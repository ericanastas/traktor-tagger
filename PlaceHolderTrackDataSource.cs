using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracktorTagger
{
    public class PlaceHolderTrackDataSource : ITrackDataSource
    {

        public PlaceHolderTrackDataSource(int total)
        {
            _total = total;
        }

        private int _total;

        public IEnumerable<TrackData> SearchTracks(string searchQuery)
        {
            for (int i = 1; i <= _total; i++)
            {

                string num = i.ToString();

                TrackData data = new TrackData("PlaceHolderTrackDataSource",
                    "id" + num,
                    "artist " + num,
                    "title " + num,
                    "mix " + num,
                    "remixer " + num,
                    "release " + num,
                    "producer " + num,
                    "label " + num,
                    "cat no " + num,
                    "lyrics " + num,
                    "genre " + num,
                     new Key('A', Accidental.Flat, Chord.Major),
                     DateTime.Now,
                     new Uri(@"http://www.google.com"));

                yield return data;
            }

        }

        public TrackData GetTrack(string trackId)
        {
            throw new NotImplementedException();
        }

        public int GetTotalResultCount(string searchQuery)
        {
            return _total;
        }
    }
}
