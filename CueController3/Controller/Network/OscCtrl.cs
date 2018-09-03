using CueController3.Model;
using Rug.Osc;
using System;
using System.ComponentModel;
using System.Diagnostics;
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

            foreach(OscTarget target in oscTargets)
            {
                if(target.id == oscCmd.id)
                {
                    using (OscSender sender = new OscSender(target.ip, target.port))
                    {
                        sender.Connect();

                        if (oscCmd.type == DataType.INT)
                            sender.Send(new OscMessage(oscCmd.oscAddress, oscCmd.intVal));

                        else if (oscCmd.type == DataType.FLOAT)
                            sender.Send(new OscMessage(oscCmd.oscAddress, oscCmd.floatVal));

                        else sender.Send(new OscMessage(oscCmd.oscAddress, oscCmd.stringVal));
                    }
                    switch(oscCmd.type)
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
            LogCtrl.Error("Couldn't find OSC target " + oscCmd.id + ".");
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
                        OscPacket packet = receiver.Receive();
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            LogCtrl.Success(packet.ToString());
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
