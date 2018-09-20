using CueController3.Controller.Dialog;
using CueController3.Controller.MyMidi;
using CueController3.Model;
using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace CueController3.Controller.Network
{
    class PbCtrl
    {

        public static void Init()
        {
            Core.win.masterIpButton.Click += MasterIpButton_Click;

            if (Properties.Settings.Default.masterIp.Length > 0 )
                Connect(Properties.Settings.Default.masterIp, Properties.Settings.Default.domain);
        }

        private static void MasterIpButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string ip = InputDialogCtrl.Show("Enter PB IP and Domain");

            if (ip != null)
            {
                string[] a = ip.Split(' ');
                if (a.Length >= 2)
                {
                    Match match = Regex.Match(a[0], @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$");

                    if (match.Success && int.TryParse(a[1], out int domain))
                    {
                        Connect(a[0], domain);
                        MidiController.Connect(ip);
                    }
                    else DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Can't connect to PB!", "You entered an invalid IP or domain.");
                }
                else DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Can't connect to PB!", "You entered an invalid IP or domain.");
            }
        }

        public static void Connect(string ip, int domain)
        {
            Auto.UnInitialize();
            Auto.InitializeTCP(ip, domain, true);
            LogCtrl.Status("Connecting to PB: " + ip + " (Domain " + domain + ")");
            Properties.Settings.Default.masterIp = ip;
            Properties.Settings.Default.domain = domain;
            Properties.Settings.Default.Save();
        }

        public static void Play(int[] seqCue)
        {
            LogCtrl.Status("Start: Sequence " + seqCue[0] + " Cue " + seqCue[1]);

            if (!Auto.MoveSequenceToCue(seqCue[0], seqCue[1]))
                LogCtrl.Error("PB Error: " + Auto.GetLastError().ToString());

            Thread.Sleep(2);

            if (!Auto.SetSequenceTransportMode(seqCue[0], "Play"))
                LogCtrl.Error("PB Error: " + Auto.GetLastError().ToString());
        }

        public static void ExecuteCmd(PbCmd cmd)
        {
            cmd.method.Invoke(null, cmd.args);

            if (Auto.GetLastError() != AutoError.None)
                LogCtrl.Error("PB Error: " + Auto.GetLastError().ToString());

            if (cmd.args.Length > 0)
                LogCtrl.Status("PB: " + cmd.method.Name + "," + string.Join(",", cmd.args));
            else LogCtrl.Status("PB: " + cmd.method.Name);
        }

        public static void ExecuteCmdArg(PbCmdArg cmdArg, float val)
        {
            try
            {
                int length = cmdArg.cmd.args.GetLength(0);
                object[] args = new object[length];
                Array.Copy(cmdArg.cmd.args, args, length);
                Type type = args[cmdArg.argNr].GetType();
                args[cmdArg.argNr] = Convert.ChangeType(val, type);
                cmdArg.cmd.method.Invoke(null, args);

                if (Auto.GetLastError() != AutoError.None)
                    LogCtrl.Error(Auto.GetLastError().ToString());
            }
            catch (Exception e)
            {
                    LogCtrl.Error(e.Message);
            }
        }

        public static void Disconnect()
        {
            Auto.UnInitialize();
        }
    }
}
