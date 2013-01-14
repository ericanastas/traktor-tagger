using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracktorTagger
{
    public static class KeyEnumList
    {
        private static IList<KeyViewModel> _keyEnumValues;

        public static IList<KeyViewModel> GetKeyEnumValues()
        {
            if(_keyEnumValues != null) return _keyEnumValues;

            _keyEnumValues = new List<KeyViewModel>();

            Type type = typeof(KeyEnum);

            _keyEnumValues.Add(new KeyViewModel(null));
            
            foreach(KeyEnum enumValue in Enum.GetValues(type)) _keyEnumValues.Add(new KeyViewModel(enumValue));

            return _keyEnumValues;
        }

    }
}
