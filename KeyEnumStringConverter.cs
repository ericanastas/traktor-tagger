using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracktorTagger
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
                            return KeyEnum.A_sharp;
                        }
                        else if(accidentalStr == "b")
                        {
                            return KeyEnum.A_flat;
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
                            return KeyEnum.A_minor;
                        }
                        else if(accidentalStr == "#")
                        {
                            return KeyEnum.A_sharp_minor;
                        }
                        else if(accidentalStr == "b")
                        {
                            return KeyEnum.A_flat_minor;
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
                            return KeyEnum.B_flat;
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
                            return KeyEnum.B_minor;
                        }

                        else if(accidentalStr == "b")
                        {
                            return KeyEnum.B_flat_minor;
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
                            return KeyEnum.C_sharp;
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
                            return KeyEnum.C_minor;
                        }
                        else if(accidentalStr == "#")
                        {
                            return KeyEnum.C_sharp_minor;
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
                            return KeyEnum.D_sharp;
                        }
                        else if(accidentalStr == "b")
                        {
                            return KeyEnum.D_flat;
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
                            return KeyEnum.D_minor;
                        }
                        else if(accidentalStr == "#")
                        {
                            return KeyEnum.D_sharp_minor;
                        }
                        else if(accidentalStr == "b")
                        {
                            return KeyEnum.D_flat_minor;
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
                            return KeyEnum.E_flat;
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
                            return KeyEnum.E_minor;
                        }

                        else if(accidentalStr == "b")
                        {
                            return KeyEnum.E_flat_minor;
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
                            return KeyEnum.E;
                        }

                        else if(accidentalStr == "b")
                        {
                            return KeyEnum.E_flat;
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
                            return KeyEnum.E_minor;
                        }

                        else if(accidentalStr == "b")
                        {
                            return KeyEnum.E_flat_minor;
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
                            return KeyEnum.G_sharp;
                        }
                        else if(accidentalStr == "b")
                        {
                            return KeyEnum.G_flat;
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
                            return KeyEnum.G_minor;
                        }
                        else if(accidentalStr == "#")
                        {
                            return KeyEnum.G_sharp_minor;
                        }
                        else if(accidentalStr == "b")
                        {
                            return KeyEnum.G_flat_minor;
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
                case KeyEnum.A_flat:
                    return "Ab";
                case KeyEnum.A:
                    return "A";
                case KeyEnum.A_sharp:
                    return "A#";
                case KeyEnum.B_flat:
                    return "Bb";
                case KeyEnum.B:
                    return "B";
                case KeyEnum.C:
                    return "C";
                case KeyEnum.C_sharp:
                    return "C#";
                case KeyEnum.D_flat:
                    return "Db";
                case KeyEnum.D:
                    return "D";
                case KeyEnum.D_sharp:
                    return "D#";
                case KeyEnum.E_flat:
                    return "Eb";
                case KeyEnum.E:
                    return "E";
                case KeyEnum.F:
                    return "F";
                case KeyEnum.F_sharp:
                    return "F#";
                case KeyEnum.G_flat:
                    return "Gb";
                case KeyEnum.G:
                    return "G";
                case KeyEnum.G_sharp:
                    return "G#";
                case KeyEnum.A_flat_minor:
                    return "Abm";
                case KeyEnum.A_minor:
                    return "Am";
                case KeyEnum.A_sharp_minor:
                    return "A#m";
                case KeyEnum.B_flat_minor:
                    return "Bbm";
                case KeyEnum.B_minor:
                    return "Bm";;
                case KeyEnum.C_minor:
                    return "Cm";;
                case KeyEnum.C_sharp_minor:
                    return "C#m";
                case KeyEnum.D_flat_minor:
                    return "Dbm";
                case KeyEnum.D_minor:
                    return "Dm";
                case KeyEnum.D_sharp_minor:
                    return "D#m";
                case KeyEnum.E_flat_minor:
                    return "Ebm";
                case KeyEnum.E_minor:
                    return "Em";
                case KeyEnum.F_minor:
                    return "Fm";
                case KeyEnum.F_sharp_minor:
                    return "F#m";
                case KeyEnum.G_flat_minor:
                    return "Gbm";
                case KeyEnum.G_minor:
                    return "Gm";
                case KeyEnum.G_sharp_minor:
                    return "G#m";
                default:
                    throw new ArgumentException("Unexpected KeyEnum value", "key");
            }

        }




    }
}
