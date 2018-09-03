using CueController3.Controller;
using System;
using System.Collections.Generic;

namespace CueController3.Model
{
    public class ControllerValues
    {
        public string[] cmds;
        public List<List<string>> ccCmdOn, ccCmdOff;
        public List<List<PbCmd>> pbCmdOn, pbCmdOff;
        public List<List<PbCmdArg>> pbCmdArgs;

        public ControllerValues(string[] _cmds)
        {
            Init();
            cmds = _cmds;

            for (int i = 0; i < 51; ++i)
            {
                string cmd = _cmds[i];
                if (string.IsNullOrWhiteSpace(cmd)) continue;

                string[] lines = cmd.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] buffArray = line.Split('-');
                    if (buffArray.Length < 2)
                    {
                        NotFound(line);
                        continue;
                    }
                    string buff1 = buffArray[1].Trim().ToUpper();
                    string buff0 = buffArray[0].Trim();

                    switch (buff1)
                    {
                        case "ON":
                            if (buff0.StartsWith("Auto"))
                            {
                                PbCmd buff = PbCmd.GetPbCmd(buff0);
                                if (buff != null) pbCmdOn[i].Add(buff);
                                else NotFound(line);
                            }
                            else
                            {
                                int pos = Array.IndexOf(ControllerLists.GetCcCmdList(), buff0);
                                if (pos > -1) ccCmdOn[i].Add(buff0);
                                else NotFound(line);
                            }
                            break;

                        case "OFF":
                            if (buff0.StartsWith("Auto"))
                            {
                                PbCmd buff = PbCmd.GetPbCmd(buff0);
                                if (buff != null) pbCmdOff[i].Add(buff);
                                else NotFound(line);
                            }
                            else
                            {
                                int pos = Array.IndexOf(ControllerLists.GetCcCmdList(), buff0);
                                if (pos > -1) ccCmdOff[i].Add(buff0);
                                else NotFound(line);
                            }
                            break;

                        default:
                            if (buff0.StartsWith("Auto"))
                            {
                                string[] vals = buff1.Split(',');
                                if (vals.Length < 3)
                                {
                                    NotFound(line);
                                    break;
                                }

                                int min, max, interp;

                                if (!int.TryParse(vals[0], out min) ||
                                    !int.TryParse(vals[1], out max) ||
                                    !int.TryParse(vals[2], out interp))
                                {
                                    NotFound(line);
                                    break;
                                }

                                PbCmdArg pbCmdArg = PbCmdArg.GetPbCmdArg(buff0, min, max, interp);
                                
                                if(pbCmdArg == null)
                                {
                                    NotFound(line);
                                    break;
                                }

                                pbCmdArgs[i].Add(pbCmdArg);
                            }
                            else NotFound(line);
                            break;
                    }
                }
            }
        }

        public ControllerValues()
        {
            Init();
        }

        private void Init()
        {
            cmds = new string[51];
            ccCmdOn = new List<List<string>>();
            ccCmdOff = new List<List<string>>();
            pbCmdOn = new List<List<PbCmd>>();
            pbCmdOff = new List<List<PbCmd>>();
            pbCmdArgs = new List<List<PbCmdArg>>();

            for (int i = 0; i < 51; ++i)
            {
                ccCmdOn.Add(new List<string>());
                ccCmdOff.Add(new List<string>());
                pbCmdOn.Add(new List<PbCmd>());
                pbCmdOff.Add(new List<PbCmd>());
                pbCmdArgs.Add(new List<PbCmdArg>());
            }
        }

        private void NotFound(string cmd)
        {
            LogCtrl.Error("Couldn't find command:");
            LogCtrl.Error(cmd);
        }
    }   
}
