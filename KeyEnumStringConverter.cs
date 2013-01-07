using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracktorTagger
{
    public static class KeyEnumStringConverter
    {
        public static KeyEnum ConvertFromString(string keyString)
        {
            if(keyString == "off") return KeyEnum.off;


            string keyStrPattern = @"^([ABCDEFG])([#b]?)(m?)";

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

        public static string ConvertToString(KeyEnum key)
        {
            return key.ToString();
        }




    }
}
