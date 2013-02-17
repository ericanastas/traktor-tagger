using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorTagger
{
    public static class RatingEnumList
    {
        private static IList<RatingViewModel> _ratingEnumValues;

        public static IList<RatingViewModel> GetRatingEnumValues()
        {
            if(_ratingEnumValues != null) return _ratingEnumValues;

            _ratingEnumValues = new List<RatingViewModel>();

            

            _ratingEnumValues.Add(new RatingViewModel(null));

            foreach(Rating enumValue in Enum.GetValues(typeof (Rating)))
            {
                _ratingEnumValues.Add(new RatingViewModel(enumValue));
            } 

            return _ratingEnumValues;
        }

    }
}
