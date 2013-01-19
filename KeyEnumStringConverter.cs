using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorTagger
{

    /// <summary>
    /// Static class which converts between Key enum values and string values stored in the NML collection file.
    /// </summary>
    public static class KeyEnumStringConverter
    {

        /// <summary>
        /// Converts a key value string from a Traktor NML file into a Key enum value
        /// </summary>
        /// <param name="keyString">The string to convert</param>
        /// <returns>Key enum value</returns>
        public static KeyEnum ConvertFromString(string keyString)
        {
            if(string.IsNullOrEmpty(keyString)) throw new ArgumentNullException("keyString");

           
            if(string.Compare(keyString, "off",StringComparison.OrdinalIgnoreCase)==0) return KeyEnum.Off;

            string keyStrPattern = @"^([ABCDEFG])([#b]?)(m?)$";

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(keyStrPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);


            var match = regex.Match(keyString);

            if(match.Success)
            {
                string letterStr = match.Groups[1].Value;
                string accidentalStr = match.Groups[2].Value;
                string chordStr = match.Groups[3].Value;


                if(letterStr == "A")
                {
                    if(string.IsNullOrEmpty(chordStr))
                    {
                        //major chord

                        if(string.IsNullOrEmpty(accidentalStr))
                        {
                            return KeyEnum.A;
                        }
                        else if(accidentalStr == "#")
                        {
                            return KeyEnum.ASharp;
                        }
                        else if(accidentalStr == "b")
                        {
                            return KeyEnum.AFlat;
                        }
                        else
                        {
                            throw new ArgumentException("Invalid accidental char: " + accidentalStr, "keyString");
                        }
                    }
                    else if(chordStr == "m")
                    {
                        //minor chord
                        if(string.IsNullOrEmpty(accidentalStr))
                        {
                            return KeyEnum.AMinor;
                        }
                        else if(accidentalStr == "#")
                        {
                            return KeyEnum.ASharpMinor;
                        }
                        else if(accidentalStr == "b")
                        {
                            return KeyEnum.AFlatMinor;
                        }
                        else
                        {
                            throw new ArgumentException("Invalid accidental char: " + accidentalStr, "keyString");
                        }

                    }
                    else
                    {
                        throw new ArgumentException("Invalid chord char: " + chordStr, "keyString");
                    }
                }
                else if(letterStr == "B")
                {
                    if(string.IsNullOrEmpty(chordStr))
                    {
                        //major chord

                        if(string.IsNullOrEmpty(accidentalStr))
                        {
                            return KeyEnum.B;
                        }

                        else if(accidentalStr == "b")
                        {
                            return KeyEnum.BFlat;
                        }
                        else
                        {
                            throw new ArgumentException("Invalid accidental char: " + accidentalStr, "keyString");
                        }
                    }
                    else if(chordStr == "m")
                    {
                        //minor chord
                        if(string.IsNullOrEmpty(accidentalStr))
                        {
                            return KeyEnum.BMinor;
                        }

                        else if(accidentalStr == "b")
                        {
                            return KeyEnum.BFlatMinor;
                        }
                        else
                        {
                            throw new ArgumentException("Invalid accidental char: " + accidentalStr, "keyString");
                        }

                    }
                    else
                    {
                        throw new ArgumentException("Invalid chord char: " + chordStr, "keyString");
                    }


                }
                else if(letterStr == "C")
                {
                    if(string.IsNullOrEmpty(chordStr))
                    {
                        //major chord

                        if(string.IsNullOrEmpty(accidentalStr))
                        {
                            return KeyEnum.C;
                        }
                        else if(accidentalStr == "#")
                        {
                            return KeyEnum.CSharp;
                        }
                        else
                        {
                            throw new ArgumentException("Invalid accidental char: " + accidentalStr, "keyString");
                        }

                    }
                    else if(chordStr == "m")
                    {
                        //minor chord
                        if(string.IsNullOrEmpty(accidentalStr))
                        {
                            return KeyEnum.CMinor;
                        }
                        else if(accidentalStr == "#")
                        {
                            return KeyEnum.CSharpMinor;
                        }
                        else
                        {
                            throw new ArgumentException("Invalid accidental char: " + accidentalStr, "keyString");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid chord char: " + chordStr, "keyString");
                    }

                }
                else if(letterStr == "D")
                {

                    if(string.IsNullOrEmpty(chordStr))
                    {
                        //major chord

                        if(string.IsNullOrEmpty(accidentalStr))
                        {
                            return KeyEnum.D;
                        }
                        else if(accidentalStr == "#")
                        {
                            return KeyEnum.DSharp;
                        }
                        else if(accidentalStr == "b")
                        {
                            return KeyEnum.DFlat;
                        }
                        else
                        {
                            throw new ArgumentException("Invalid accidental char: " + accidentalStr, "keyString");
                        }
                    }
                    else if(chordStr == "m")
                    {
                        //minor chord
                        if(string.IsNullOrEmpty(accidentalStr))
                        {
                            return KeyEnum.DMinor;
                        }
                        else if(accidentalStr == "#")
                        {
                            return KeyEnum.DSharpMinor;
                        }
                        else if(accidentalStr == "b")
                        {
                            return KeyEnum.DFlatMinor;
                        }
                        else
                        {
                            throw new ArgumentException("Invalid accidental char: " + accidentalStr, "keyString");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid chord char: " + chordStr, "keyString");
                    }
                }
                else if(letterStr == "E")
                {
                    if(string.IsNullOrEmpty(chordStr))
                    {
                        //major chord

                        if(string.IsNullOrEmpty(accidentalStr))
                        {
                            return KeyEnum.E;
                        }

                        else if(accidentalStr == "b")
                        {
                            return KeyEnum.EFlat;
                        }
                        else
                        {
                            throw new ArgumentException("Invalid accidental char: " + accidentalStr, "keyString");
                        }
                    }
                    else if(chordStr == "m")
                    {
                        //minor chord
                        if(string.IsNullOrEmpty(accidentalStr))
                        {
                            return KeyEnum.EMinor;
                        }

                        else if(accidentalStr == "b")
                        {
                            return KeyEnum.EFlatMinor;
                        }
                        else
                        {
                            throw new ArgumentException("Invalid accidental char: " + accidentalStr, "keyString");
                        }

                    }
                    else
                    {
                        throw new ArgumentException("Invalid chord char: " + chordStr, "keyString");
                    }

                }
                else if(letterStr == "F")
                {

                    if(string.IsNullOrEmpty(chordStr))
                    {
                        //major chord

                        if(string.IsNullOrEmpty(accidentalStr))
                        {
                            return KeyEnum.F;
                        }
                        else if(accidentalStr == "#")
                        {
                            return KeyEnum.FSharp;
                        }
                        else
                        {
                            throw new ArgumentException("Invalid accidental char: " + accidentalStr, "keyString");
                        }
                    }
                    else if(chordStr == "m")
                    {
                        //minor chord
                        if(string.IsNullOrEmpty(accidentalStr))
                        {
                            return KeyEnum.FMinor;
                        }
                        else if(accidentalStr == "#")
                        {
                            return KeyEnum.FSharpMinor;
                        }
                        else
                        {
                            throw new ArgumentException("Invalid accidental char: " + accidentalStr, "keyString");
                        }

                    }
                    else
                    {
                        throw new ArgumentException("Invalid chord char: " + chordStr, "keyString");
                    }
                }
                else if(letterStr == "G")
                {
                    if(string.IsNullOrEmpty(chordStr))
                    {
                        //major chord

                        if(string.IsNullOrEmpty(accidentalStr))
                        {
                            return KeyEnum.G;
                        }
                        else if(accidentalStr == "#")
                        {
                            return KeyEnum.GSharp;
                        }
                        else if(accidentalStr == "b")
                        {
                            return KeyEnum.GFlat;
                        }
                        else
                        {
                            throw new ArgumentException("Invalid accidental char: " + accidentalStr, "keyString");
                        }
                    }
                    else if(chordStr == "m")
                    {
                        //minor chord
                        if(string.IsNullOrEmpty(accidentalStr))
                        {
                            return KeyEnum.GMinor;
                        }
                        else if(accidentalStr == "#")
                        {
                            return KeyEnum.GSharpMinor;
                        }
                        else if(accidentalStr == "b")
                        {
                            return KeyEnum.GFlatMinor;
                        }
                        else
                        {
                            throw new ArgumentException("Invalid accidental char: " + accidentalStr, "keyString");
                        }

                    }
                    else
                    {
                        throw new ArgumentException("Invalid chord char: " + chordStr, "keyString");
                    }

                }
                else //not one of the supported letters
                {
                    throw new ArgumentException("Invalid key char: " + letterStr, "keyString");
                }
            }
            else //match was not a success
            {
                throw new ArgumentException("Invalid keystring format: " + keyString, "keyString");
            }

        }



        /// <summary>
        /// Converts a key enum value into a key string to be stored in a Traktor NML file
        /// </summary>
        /// <param name="key">Key to convert</param>
        /// <returns>Key string value</returns>
        public static string ConvertToString(KeyEnum key)
        {
            switch(key)
            {
                case KeyEnum.Off:
                    return "Off";
                case KeyEnum.AFlat:
                    return "Ab";
                case KeyEnum.A:
                    return "A";
                case KeyEnum.ASharp:
                    return "A#";
                case KeyEnum.BFlat:
                    return "Bb";
                case KeyEnum.B:
                    return "B";
                case KeyEnum.C:
                    return "C";
                case KeyEnum.CSharp:
                    return "C#";
                case KeyEnum.DFlat:
                    return "Db";
                case KeyEnum.D:
                    return "D";
                case KeyEnum.DSharp:
                    return "D#";
                case KeyEnum.EFlat:
                    return "Eb";
                case KeyEnum.E:
                    return "E";
                case KeyEnum.F:
                    return "F";
                case KeyEnum.FSharp:
                    return "F#";
                case KeyEnum.GFlat:
                    return "Gb";
                case KeyEnum.G:
                    return "G";
                case KeyEnum.GSharp:
                    return "G#";
                case KeyEnum.AFlatMinor:
                    return "Abm";
                case KeyEnum.AMinor:
                    return "Am";
                case KeyEnum.ASharpMinor:
                    return "A#m";
                case KeyEnum.BFlatMinor:
                    return "Bbm";
                case KeyEnum.BMinor:
                    return "Bm";;
                case KeyEnum.CMinor:
                    return "Cm";;
                case KeyEnum.CSharpMinor:
                    return "C#m";
                case KeyEnum.DFlatMinor:
                    return "Dbm";
                case KeyEnum.DMinor:
                    return "Dm";
                case KeyEnum.DSharpMinor:
                    return "D#m";
                case KeyEnum.EFlatMinor:
                    return "Ebm";
                case KeyEnum.EMinor:
                    return "Em";
                case KeyEnum.FMinor:
                    return "Fm";
                case KeyEnum.FSharpMinor:
                    return "F#m";
                case KeyEnum.GFlatMinor:
                    return "Gbm";
                case KeyEnum.GMinor:
                    return "Gm";
                case KeyEnum.GSharpMinor:
                    return "G#m";
                default:
                    throw new ArgumentException("Unexpected KeyEnum value", "key");
            }

        }




    }
}
