using System.Collections.Generic;

namespace CueController3.Model
{
    class Script
    {
        public string title { get; }
        public List<Cue> cues { get; }
        public string scriptString { get; }

        public Script(string title, string[] lines, string scriptString)
        {
            List<Cue> buff = new List<Cue>();
            Cue cue = new Cue();
            this.title = title;

            foreach(string line in lines)
            {
                if (line.StartsWith("PB"))
                {
                    string[] sBuff = line.Split(' ');
                    if (sBuff.Length >= 2) cue = new Cue() { Description = sBuff[1], SendString = sBuff[0] };
                }
                else if(line.StartsWith("OSC"))
                {
                    string[] sBuff = line.Split(' ');
                    if (sBuff.Length >= 4 && sBuff[2].StartsWith("/") )
                        cue = new Cue() { Description = sBuff[2] + " " + sBuff[3], SendString = sBuff[0] + " " + sBuff[1]};
                }
                else cue = new Cue() { SendString = line };
                if (cue.send != null) buff.Add(cue);
            }
            cues = buff;
            this.scriptString = scriptString;
        }
    }
}