namespace CueController3.Model
{
   public class PbCmdArg
    {
        public PbCmd cmd;
        public int argNr, min, max, interp;
        public float val, step;
        public string name;

        private PbCmdArg(PbCmd cmd, int argNr, string name, int min, int max, int interp)
        {
            this.cmd = cmd;
            this.argNr = argNr;
            this.name = name;
            this.min = min;
            this.max = max;
            this.interp = interp;
            val = min;
        }

        public static PbCmdArg GetPbCmdArg(string cmd, int min, int max, int interp)
        {
            string[] cmdArray = cmd.Split(',');
            if (cmdArray.Length <= 1) return null;

            int nr = -1;
            for (int i = 1; i < cmdArray.Length; ++i)
            {
                if (cmdArray[i].ToLower() == "val")
                {
                    nr = i-1;
                    break;
                }
            }
            if (nr == -1) return null;

            string buff = cmd.Replace("val", "0");
            PbCmd pbCmd = PbCmd.GetPbCmd(buff);
            return pbCmd == null || nr >= pbCmd.args.Length  ?  null : new PbCmdArg(pbCmd, nr, cmd, min, max, interp);
        }
    }
}
