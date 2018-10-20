using CueController3.Controller.Cues;
using CueController3.Controller.Dialog;
using CueController3.Model;
using Rug.Osc;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
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
            Core.win.setOscPortButton.Click += SetOscPortButton_Click;
            Connect();
        }

        private static void SetOscPortButton_Click(object sender, RoutedEventArgs e)
        {
            string s = InputDialogCtrl.Show("Enter: OSC receive Port", 300);

            if (s != null)
            {
                if (int.TryParse(s, out int port))
                {
                    Properties.Settings.Default.oscReceivePort = port;
                    Properties.Settings.Default.Save();
                    Close();
                    Connect();
                }
                else DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Wrong Format!", "Enter OSC port as a single number.");
            }
        }

        private static void Connect()
        {
            LogCtrl.Status("Set OSC input port: " + Properties.Settings.Default.oscReceivePort);
            receiver = new OscReceiver(Properties.Settings.Default.oscReceivePort);
            receiver.Connect();
            oscWorker = new BackgroundWorker();
            oscWorker.WorkerSupportsCancellation = true;
            oscWorker.DoWork += OscWorker_DoWork;
            oscWorker.RunWorkerAsync();
        }

        public static void Close()
        {
            try
            {
                oscWorker.CancelAsync();
                receiver.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static void Send(OscCmd oscCmd)
        {
            if (oscMute)
            {
                switch (oscCmd.type)
                {
                    case DataType.FLOAT:
                        LogCtrl.Warning(string.Concat("Send OSC: ", oscCmd.oscAddress, " ", oscCmd.floatVal));
                        break;
                    case DataType.INT:
                        LogCtrl.Warning(string.Concat("Send OSC: ", oscCmd.oscAddress, " ", oscCmd.intVal));
                        break;
                    default:
                        LogCtrl.Warning(string.Concat("Send OSC: ", oscCmd.oscAddress, " ", oscCmd.stringVal));
                        break;
                }
                return;
            }

            OscTarget target = oscTargets.Find(x => x.keyword == oscCmd.keyword);
            if (target != null)
            {
                new Thread(() => SendOSC(oscCmd, target)).Start();

                switch (oscCmd.type)
                {
                    case DataType.FLOAT:
                        LogCtrl.Status(string.Concat("Send OSC: ", target.prefix, oscCmd.oscAddress, " ", oscCmd.floatVal));
                        break;
                    case DataType.INT:
                        LogCtrl.Status(string.Concat("Send OSC: ", target.prefix, oscCmd.oscAddress, " ", oscCmd.intVal));
                        break;
                    default:
                        LogCtrl.Status(string.Concat("Send OSC: ", target.prefix, oscCmd.oscAddress, " ", oscCmd.stringVal));
                        break;
                }
                return;
            }

            LogCtrl.Error(string.Concat("Couldn't find OSC target ", oscCmd.keyword, "."));
        }

        public static void SendOSC(OscCmd oscCmd, OscTarget target)
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
        }

        public static void OscMute(bool b)
        {
            oscMute = b;
            if (oscMute) Core.win.oscMuteButton.Background = new SolidColorBrush(Colors.Red);
            else Core.win.oscMuteButton.Background = new SolidColorBrush(Colors.Transparent);
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
                                    Match match = Regex.Match(command, @".*/(\d+(\.\d+)?)/fire.*$");

                                    if (match.Success)
                                    {
                                        for (int i = 0; i < CuelistCtrl.cues.Count; ++i)
                                        {
                                            Cue cue = CuelistCtrl.cues[i];
                                            if (cue.trigger != null && cue.trigger.type == TriggerType.OSC && cue.trigger.oscCmd.stringVal == match.Groups[1].Value)
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
                if (!oscWorker.CancellationPending)
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    LogCtrl.Error(ex.Message);
                }));
            }
        }
    }
}
