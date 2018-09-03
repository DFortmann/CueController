using System.Text.RegularExpressions;

namespace CueController3.Model
{
    class MidiNote
    {
        public int pitch;
        public string note;

        private MidiNote(int pitch, string note)
        {
            this.pitch = pitch;
            this.note = note;
        }

        public static MidiNote getMidiNote(string _note)
        {
            int p;

            if (int.TryParse(_note, out p))
                return getMidiNote(p);
            else
            {
                string no = _note.ToUpper();
                int po = getNote(no);
                if (po >= 0) return new MidiNote(po, no);
                else return null;
            }
        }

        public static MidiNote getMidiNote(int pitch)
        {
            string n = getNote(pitch);
            if (n != null) return new MidiNote(pitch, n);
            return null;
        }


        private static string getNote(int noteInt)
        {
            int pitch = noteInt % 12;
            int oct = noteInt / 12;
            string noteString;

            switch (pitch)
            {
                case 0:
                    noteString = "C";
                    break;
                case 1:
                    noteString = "C#";
                    break;
                case 2:
                    noteString = "D";
                    break;
                case 3:
                    noteString = "D#";
                    break;
                case 4:
                    noteString = "E";
                    break;
                case 5:
                    noteString = "F";
                    break;
                case 6:
                    noteString = "F#";
                    break;
                case 7:
                    noteString = "G";
                    break;
                case 8:
                    noteString = "G#";
                    break;
                case 9:
                    noteString = "A";
                    break;
                case 10:
                    noteString = "A#";
                    break;
                case 11:
                    noteString = "B";
                    break;
                default: return null;
            }
            noteString += (oct - 2);
            return noteString;
        }

        private static int getNote(string _note)
        {
            Match match = Regex.Match(_note, @"([A-H]#?)(-?[0-7])", RegexOptions.IgnoreCase);

            if (match.Success && match.Groups.Count >= 3)
            {
                string note = match.Groups[1].Value;
                int pitch, octave;

                if (int.TryParse(match.Groups[2].Value, out octave))
                {
                    switch (note)
                    {
                        case "C":
                            pitch = 0;
                            break;
                        case "C#":
                            pitch = 1;
                            break;
                        case "D":
                            pitch = 2;
                            break;
                        case "D#":
                            pitch = 3;
                            break;
                        case "E":
                            pitch = 4;
                            break;
                        case "F":
                            pitch = 5;
                            break;
                        case "F#":
                            pitch = 6;
                            break;
                        case "G":
                            pitch = 7;
                            break;
                        case "G#":
                            pitch = 8;
                            break;
                        case "A":
                            pitch = 9;
                            break;
                        case "A#":
                            pitch = 10;
                            break;
                        case "B":
                            pitch = 11;
                            break;
                        default:
                            return -1;
                    }
                    return pitch + (octave + 2) * 12;
                }
                else return -1;    
            }
            else return -1;
        }
    }
}
