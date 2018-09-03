using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Markup;

namespace CueController3.Model
{
    public enum TriggerType {FOLLOW, MSC, NOTE }

    class Trigger
    {
        public long followTime;
        public TriggerType type;
        public MscCommand  mscCmd;
        public MidiNote note;
        public string name;
        public Canvas icon;

        private Trigger(MscCommand cmd)
        {
            type = TriggerType.MSC;
            mscCmd = cmd;
            name = cmd.message;
            icon = XamlReader.Parse(Properties.Resources.light) as Canvas;
        }

        private Trigger(long followTime, string name)
        {
            type = TriggerType.FOLLOW;
            this.name = name;
            this.followTime = followTime;
            icon = XamlReader.Parse(Properties.Resources.clock) as Canvas;
        }

        private Trigger(MidiNote note, string name)
        {
            type = TriggerType.NOTE;
            this.note = note;
            this.name = name; 
            icon = XamlReader.Parse(Properties.Resources.note) as Canvas;
        }

        public static Trigger GetTrigger(string value) {

            string[] array = value == null ? new string[] { } : value.Trim().Split(' ');

            if (array.Length == 2)
            {
                switch (array[0].ToUpper())
                {
                    case "CUE":
                        MscCommand mscCmd1 = MscCommand.getCue(array[1]);
                        if (mscCmd1 != null) return new Trigger(mscCmd1);
                        break;

                    case "FOLLOW":
                        long buff = 0;
                        bool success = false;

                        Match match = Regex.Match(array[1], @"(\d*):([0-5][0-9])\.(\d{3})");
                        if (match.Success)
                        {
                            buff = long.Parse(match.Groups[1].Value) * 60000;
                            buff += long.Parse(match.Groups[2].Value) * 1000;
                            buff += long.Parse(match.Groups[3].Value);
                            success = true;
                        }
                        else
                        {
                            match = Regex.Match(array[1], @"([0-5]?[0-9])\.(\d{3})");
                            if (match.Success)
                            {
                                buff += long.Parse(match.Groups[1].Value) * 1000;
                                buff += long.Parse(match.Groups[2].Value);
                                success = true;
                            }
                            else
                            {
                                match = Regex.Match(array[1], @"(\d{1,3})");
                                if (match.Success)
                                {
                                    buff += long.Parse(match.Groups[1].Value);
                                    success = true;
                                }
                            }
                        }
                        if (success) return new Trigger(buff, "Follow " + match.Groups[0].Value);
                        break;

                    case "MACRO":
                        MscCommand mscCmd2 = MscCommand.getMacro(array[1]);
                        if (mscCmd2 != null) return new Trigger(mscCmd2);
                        break;

                    case "NOTE":
                        MidiNote note = MidiNote.getMidiNote(array[1]);
                        if (note != null && note.pitch < 108) return new Trigger(note, "Note " + array[1]);
                        break;
                }
            }
            return null;
        }
    }
}
