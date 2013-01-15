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
        public static Key ConvertFromString(string keyString)
        {
            if(string.IsNullOrEmpty(keyString)) throw new ArgumentNullException("keyString");

           
            if(string.Compare(keyString, "off",StringComparison.OrdinalIgnoreCase)==0) return Key.Off;

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
                            return Key.A;
                        }
                        else if(accidentalStr == "#")
                        {
                            return Key.ASharp;
                        }
                        else if(accidentalStr == "b")
                        {
                            return Key.AFlat;
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
                            return Key.AMinor;
                        }
                        else if(accidentalStr == "#")
                        {
                            return Key.ASharpMinor;
                        }
                        else if(accidentalStr == "b")
                        {
                            return Key.AFlatMinor;
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
                            return Key.B;
                        }

                        else if(accidentalStr == "b")
                        {
                            return Key.BFlat;
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
                            return Key.BMinor;
                        }

                        else if(accidentalStr == "b")
                        {
                            return Key.BFlatMinor;
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
                            return Key.C;
                        }
                        else if(accidentalStr == "#")
                        {
                            return Key.CSharp;
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
                            return Key.CMinor;
                        }
                        else if(accidentalStr == "#")
                        {
                            return Key.CSharpMinor;
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
                            return Key.D;
                        }
                        else if(accidentalStr == "#")
                        {
                            return Key.DSharp;
                        }
                        else if(accidentalStr == "b")
                        {
                            return Key.DFlat;
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
                            return Key.DMinor;
                        }
                        else if(accidentalStr == "#")
                        {
                            return Key.DSharpMinor;
                        }
                        else if(accidentalStr == "b")
                        {
                            return Key.DFlatMinor;
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
                            return Key.E;
                        }

                        else if(accidentalStr == "b")
                        {
                            return Key.EFlat;
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
                            return Key.EMinor;
                        }

                        else if(accidentalStr == "b")
                        {
                            return Key.EFlatMinor;
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
                            return Key.F;
                        }
                        else if(accidentalStr == "#")
                        {
                            return Key.FSharp;
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
                            return Key.FMinor;
                        }
                        else if(accidentalStr == "#")
                        {
                            return Key.FSharpMinor;
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
                            return Key.G;
                        }
                        else if(accidentalStr == "#")
                        {
                            return Key.GSharp;
                        }
                        else if(accidentalStr == "b")
                        {
                            return Key.GFlat;
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
                            return Key.GMinor;
                        }
                        else if(accidentalStr == "#")
                        {
                            return Key.GSharpMinor;
                        }
                        else if(accidentalStr == "b")
                        {
                            return Key.GFlatMinor;
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
        public static string ConvertToString(Key key)
        {
            switch(key)
            {
                case Key.Off:
                    return "Off";
                case Key.AFlat:
                    return "Ab";
                case Key.A:
                    return "A";
                case Key.ASharp:
                    return "A#";
                case Key.BFlat:
                    return "Bb";
                case Key.B:
                    return "B";
                case Key.C:
                    return "C";
                case Key.CSharp:
                    return "C#";
                case Key.DFlat:
                    return "Db";
                case Key.D:
                    return "D";
                case Key.DSharp:
                    return "D#";
                case Key.EFlat:
                    return "Eb";
                case Key.E:
                    return "E";
                case Key.F:
                    return "F";
                case Key.FSharp:
                    return "F#";
                case Key.GFlat:
                    return "Gb";
                case Key.G:
                    return "G";
                case Key.GSharp:
                    return "G#";
                case Key.AFlatMinor:
                    return "Abm";
                case Key.AMinor:
                    return "Am";
                case Key.ASharpMinor:
                    return "A#m";
                case Key.BFlatMinor:
                    return "Bbm";
                case Key.BMinor:
                    return "Bm";;
                case Key.CMinor:
                    return "Cm";;
                case Key.CSharpMinor:
                    return "C#m";
                case Key.DFlatMinor:
                    return "Dbm";
                case Key.DMinor:
                    return "Dm";
                case Key.DSharpMinor:
                    return "D#m";
                case Key.EFlatMinor:
                    return "Ebm";
                case Key.EMinor:
                    return "Em";
                case Key.FMinor:
                    return "Fm";
                case Key.FSharpMinor:
                    return "F#m";
                case Key.GFlatMinor:
                    return "Gbm";
                case Key.GMinor:
                    return "Gm";
                case Key.GSharpMinor:
                    return "G#m";
                default:
                    throw new ArgumentException("Unexpected KeyEnum value", "key");
            }

        }




    }
}
