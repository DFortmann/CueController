using System;
using System.Text.RegularExpressions;

namespace CueController3.Model
{
    public enum SendType { SC, SCRIPT, SHUTTER, MATRIX, MSC, MUTE, NOTE, PB, OSC }
    public enum MuteType { ALL, NOTE, MSC, OSC }

    class Send
    {
        public string name;
        public int[] pbCue;
        public MatrixCmd matrixCmd;
        public int scriptNr;
        public Tuple<int, bool> beamerShutter;
        public Tuple<MuteType, bool> mute;
        public PbCmd pbCmd;
        public MscCommand mscCmd;
        public MidiNote note;
        public OscCmd oscCmd;
        public SendType type;

        private Send(Tuple<int, bool> beamerShutter, string name)
        {
            this.beamerShutter = beamerShutter;
            this.name = name;
            type = SendType.SHUTTER;
        }

        private Send(MscCommand mscCmd)
        {
            type = SendType.MSC;
            this.mscCmd = mscCmd;
            name = mscCmd.message;
        }

        private Send(MatrixCmd matrixCmd)
        {
            type = SendType.MATRIX;
            this.matrixCmd = matrixCmd;
            name = "Matrix " + matrixCmd.name;
        }

        private Send(Tuple<MuteType, bool> mute, string name)
        {
            type = SendType.MUTE;
            this.mute = mute;
            this.name = name;
        }

        private Send(MidiNote note, string name)
        {
            type = SendType.NOTE;
            this.note = note;
            this.name = name;
        }

        private Send(int sequence, int cue, string name)
        {
            type = SendType.SC;
            pbCue = new int[] { sequence, cue };
            this.name = name;
        }

        private Send(int scriptNr, string name)
        {
            type = SendType.SCRIPT;
            this.scriptNr = scriptNr;
            this.name = name;
        }

        private Send(PbCmd pbCmd)
        {
            type = SendType.PB;
            this.pbCmd = pbCmd;
            name = "PB";
        }

        private Send(OscCmd oscCmd)
        {
            type = SendType.OSC;
            this.oscCmd = oscCmd;
            name = "OSC";
        }

        public static Send GetSend(string value, string pbScript)
        {
            string[] array = value == null ? new string[] { } : value.Trim().Split(' ');
            if (array.Length >= 2)
            {
                Match match = Regex.Match(array[0], @"B(\d+)", RegexOptions.IgnoreCase);
                if (match.Success && match.Groups.Count == 2)
                {
                    if (array[1].ToUpper() == "CLOSE")
                        return new Send(new Tuple<int, bool>(int.Parse(match.Groups[1].Value), false), array[0].ToUpper() + " close");
                    else if (array[1].ToUpper() == "OPEN")
                        return new Send(new Tuple<int, bool>(int.Parse(match.Groups[1].Value), true), array[0].ToUpper() + " open");
                }
                else
                {
                    switch (array[0].ToUpper())
                    {
                        case "CUE":
                            MscCommand mscCmd1 = MscCommand.getCue(array[1]);
                            if (mscCmd1 != null) return new Send(mscCmd1);
                            break;

                        case "MACRO":
                            MscCommand mscCmd2 = MscCommand.getMacro(array[1]);
                            if (mscCmd2 != null) return new Send(mscCmd2);
                            break;

                        case "MATRIX":
                            MatrixCmd matrixCmd = MatrixCmd.getMatrixCmd(array[1]);
                            if (matrixCmd != null) return new Send(matrixCmd);
                            break;

                        case "MUTE":
                            switch (array[1].ToUpper())
                            {
                                case "ALL":
                                    return new Send(new Tuple<MuteType, bool>(MuteType.ALL, true), "Mute All");
                                case "NOTE":
                                    return new Send(new Tuple<MuteType, bool>(MuteType.NOTE, true), "Mute Note");
                                case "MSC":
                                    return new Send(new Tuple<MuteType, bool>(MuteType.MSC, true), "Mute MSC");
                                case "OSC":
                                    return new Send(new Tuple<MuteType, bool>(MuteType.OSC, true), "Mute OSC");
                            }
                            break;

                        case "UNMUTE":
                            switch (array[1].ToUpper())
                            {
                                case "ALL":
                                    return new Send(new Tuple<MuteType, bool>(MuteType.ALL, false), "Unmute All");
                                case "NOTE":
                                    return new Send(new Tuple<MuteType, bool>(MuteType.NOTE, false), "Unmute Note");
                                case "MSC":
                                    return new Send(new Tuple<MuteType, bool>(MuteType.MSC, false), "Unmute MSC");
                                case "OSC":
                                    return new Model.Send(new Tuple<MuteType, bool>(MuteType.OSC, false), "Unmute OSC");
                            }
                            break;

                        case "NOTE":
                            MidiNote note = MidiNote.getMidiNote(array[1]);
                            if (note != null && note.pitch < 108) return new Send(note, "Note " + array[1]);
                            break;

                        case "SC":
                            string[] s = array[1].Split(',');
                            if (s.Length >= 2)
                            {
                                int sequence, cue;
                                if (int.TryParse(s[0], out sequence) && int.TryParse(s[1], out cue))
                                    return new Send(sequence, cue, "SC " + array[1]);
                            }
                            break;

                        case "SCRIPT":
                            int scriptNr;
                            if (int.TryParse(array[1], out scriptNr) && scriptNr > 0 && scriptNr < 11)
                                return new Send(scriptNr, "Script " + array[1]);
                            break;
                    }
                }
            }
            else if (array.Length == 1)
            {
                if (array[0].ToUpper() == "PB" && pbScript != null)
                {
                    PbCmd pbCmd = PbCmd.GetPbCmd(pbScript);
                    if (pbCmd != null) return new Send(pbCmd);
                }

                else if (array[0].ToUpper() == "OSC" && pbScript != null)
                {
                    OscCmd oscCmd = OscCmd.GetOscCmd(pbScript);
                    if (oscCmd != null) return new Send(oscCmd);
                }
            }
            return null;
        }
    }
}
