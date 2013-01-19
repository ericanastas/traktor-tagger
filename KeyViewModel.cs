using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorTagger
{
    public class KeyViewModel
    {
        public KeyEnum? Key { get; private set; }
        public string Description { get; private set; }

        public KeyViewModel(KeyEnum? key)
        {
            Key = key;

            if(Key.HasValue)
            {
                Description = KeyEnumStringConverter.ConvertToString(key.Value);
            }
            else
            {
                Description = "";
            }
        }
    }
}
