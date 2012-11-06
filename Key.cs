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

        public Key(char letter, Accidental accidental, Chord chord)
        {
            //if (letter == null) throw new ArgumentNullException("letter");

            Letter = letter;
            this.Accidental = accidental;
            this.Chord = chord;
        }

        public override string ToString()
        {
            string returnString = String.Empty;

            StringBuilder sb = new StringBuilder();

            sb.Append(Letter);


            switch (Accidental)
            {
                case Accidental.Flat:

                    char flat = Char.ConvertFromUtf32(9837)[0];

                    sb.Append(flat);

                    break;
                case Accidental.Sharp:

                    char sharp = Char.ConvertFromUtf32(9839)[0];

                    sb.Append(sharp);
                    break;
            }

            sb.Append(" ");


            switch (Chord)
            {
                case Chord.Major:
                    sb.Append("Major");
                    break;
                case Chord.Minor:
                    sb.Append("Minor");
                    break;
            }

            return sb.ToString();            
        }


        public Key(string keyString)
        {
            throw new NotImplementedException();
        }

       
    }
}
