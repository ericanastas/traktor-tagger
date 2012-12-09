using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracktorTagger
{
    public enum Accidental
    {
        Natural,
        Flat,
        Sharp
    }

    public enum Chord
    {
        Major,
        Minor
    }

    public class Key
    {

        public Accidental Accidental { get; private set; }
        public Chord Chord { get; private set; }
        public char Letter { get; private set; }


        private const string approvedLetters = "abcdefg";


        public Key(string key)
        {
            string keyStrPattern = @"^([ABCDEFG])([#b]?)(m?)";

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(keyStrPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);


            var match = regex.Match(key);

            if (match.Success)
            {
                string letterStr = match.Groups[1].Value;
                string accidentalStr = match.Groups[2].Value;
                string chordStr = match.Groups[3].Value;


                Accidental acc = TracktorTagger.Accidental.Natural;

                if (accidentalStr == "#")
                {
                    acc = TracktorTagger.Accidental.Sharp;
                }
                else if (accidentalStr == "b")
                {
                    acc = TracktorTagger.Accidental.Flat;
                }

                Chord c = TracktorTagger.Chord.Major;
                if (chordStr == "m") c = TracktorTagger.Chord.Minor;



                Letter = letterStr.ToUpper()[0];
                this.Accidental = acc;
                this.Chord = c;

            }
            else
            {
                throw new ArgumentException("Invalid key string format", "key");
            }


        }

        public Key(char letter, Accidental accidental, Chord chord)
        {
            string letterString = letter.ToString();

            if (approvedLetters.IndexOf(letterString, StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new ArgumentException("Letter must be a, b, c, d, e, f, or g ", "letter");
            }

            Letter = letterString.ToUpper()[0];
            this.Accidental = accidental;
            this.Chord = chord;
        }

        public override string ToString()
        {
            string returnString = String.Empty;

            StringBuilder sb = new StringBuilder();

            sb.Append(Letter);

            if (this.Accidental == TracktorTagger.Accidental.Flat) sb.Append("b");
            else if (this.Accidental == TracktorTagger.Accidental.Sharp) sb.Append("#");


            if (this.Chord == TracktorTagger.Chord.Minor) sb.Append("m");

            return sb.ToString();
        }

    }
}
