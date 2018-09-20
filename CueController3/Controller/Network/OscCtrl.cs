using CueController3.Controller.Cues;
using CueController3.Model;
using Rug.Osc;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using static CueController3.Controller.Network.OscListCtrl;

namespace CueController3.Controller.Network
{
    class OscCtrl
    {
        public enum DataType { STRING, INT, FLOAT }
        public static bool oscMute = true;
        private static OscReceiver receiver;
        private static BackgroundWorker oscWorker;

        public static void Init()
        {
            receiver = new OscReceiver(9993);
            receiver.Connect();
            oscWorker = new BackgroundWorker();
            oscWorker.DoWork += OscWorker_DoWork;
            oscWorker.RunWorkerAsync();
        }

        public static void Send(OscCmd oscCmd)
        {
            if (oscMute)
            {
                switch (oscCmd.type)
                {
                    case DataType.FLOAT:
                        LogCtrl.Warning("Send OSC: " + oscCmd.oscAddress + " " + oscCmd.floatVal);
                        break;
                    case DataType.INT:
                        LogCtrl.Warning("Send OSC: " + oscCmd.oscAddress + " " + oscCmd.intVal);
                        break;
                    default:
                        LogCtrl.Warning("Send OSC: " + oscCmd.oscAddress + " " + oscCmd.stringVal);
                        break;
                }
                return;
            }

            foreach (OscTarget target in oscTargets)
            {
                if (target.keyword == oscCmd.keyword)
                {
                    using (OscSender sender = new OscSender(target.ip, target.port))
                    {
                        sender.Connect();

                        if (oscCmd.type == DataType.INT)
                            sender.Send(new OscMessage(target.prefix + oscCmd.oscAddress, oscCmd.intVal));

                        else if (oscCmd.type == DataType.FLOAT)
                            sender.Send(new OscMessage(target.prefix + oscCmd.oscAddress, oscCmd.floatVal));

                        else sender.Send(new OscMessage(target.prefix + oscCmd.oscAddress, oscCmd.stringVal));
                    }
                    switch (oscCmd.type)
                    {
                        case DataType.FLOAT:
                            LogCtrl.Status("Send OSC: " + oscCmd.oscAddress + " " + oscCmd.floatVal);
                            break;
                        case DataType.INT:
                            LogCtrl.Status("Send OSC: " + oscCmd.oscAddress + " " + oscCmd.intVal);
                            break;
                        default:
                            LogCtrl.Status("Send OSC: " + oscCmd.oscAddress + " " + oscCmd.stringVal);
                            break;
                    }
                    return;
                }
            }
            LogCtrl.Error("Couldn't find OSC target " + oscCmd.keyword + ".");
        }

        public static void OscMute(bool b)
        {
            oscMute = b;
            if (oscMute) Core.win.oscMuteButton.Background = new SolidColorBrush(Colors.Red);
            else Core.win.oscMuteButton.Background = new SolidColorBrush(Colors.Transparent);
        }

        public static void Close()
        {
            try
            {
                receiver.Close();
                oscWorker.CancelAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        private static void OscWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (receiver.State != OscSocketState.Closed)
                {
                    if (oscWorker.CancellationPending) return;

                    if (receiver.State == OscSocketState.Connected)
                    {
                        string command = receiver.Receive().ToString();

                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            if (!oscMute)
                            {
                                if ((command.StartsWith("/video") || command.StartsWith("/eos/out")))
                                {
                                    Match match = Regex.Match(command, @".*/(\d+)/fire.*$");
                                    if (match.Success)
                                    {
                                        Debug.WriteLine(match.Groups[1].Value);
                                        if (int.TryParse(match.Groups[1].Value, out int cueNr))
                                        {
                                            for (int i = 0; i < CuelistCtrl.cues.Count; ++i)
                                            {
                                                Cue cue = CuelistCtrl.cues[i];
                                                if (cue.trigger != null && cue.trigger.type == TriggerType.OSC && cue.trigger.oscCmd.intVal == cueNr)
                                                {
                                                    if (Core.win.saveTriggerCheckbox.IsChecked && Core.win.cueTable.SelectedIndex != i)
                                                        LogCtrl.Error("In: " + command);
                                                    else
                                                    {
                                                        LogCtrl.Success("In: " + command);
                                                        GoCtrl.Go(i);
                                                    }
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                    LogCtrl.Status("In: " + command);
                                }
                                else LogCtrl.Status("In: " + command);
                            }
                            else LogCtrl.Warning("In: " + command);
                        
                        }));
                    }
                }

            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    LogCtrl.Error(ex.Message);
                }));
            }
        }
    }
}
