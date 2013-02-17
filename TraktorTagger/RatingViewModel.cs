using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorTagger
{
    public class RatingViewModel
    {
        public Rating? Rating { get; private set; }
        public string Description {get; private set;}




        public RatingViewModel(Rating? r)
        {
            this.Rating = r;


            Rating = r;

            if(r.HasValue)
            {
                Description = r.Value.ToString();
            }
            else
            {
                Description = String.Empty;
            }
        }
    }
}
