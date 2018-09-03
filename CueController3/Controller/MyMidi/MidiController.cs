using CueController3.Controller.Cues;
using CueController3.Controller.Network;
using CueController3.Controller.Scripts;
using CueController3.Model;
using CueController3.View;
using Midi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading;
using System.Windows;

namespace CueController3.Controller.MyMidi
{
    class MidiController
    {
        private static ControllerValues ctrlVals;
        private static ReadOnlyCollection<InputDevice> midiInputs;
        private static InputDevice inputDevice;
        private static Thread interpThread;
        private static MidiCtrlDialog dialog;
        private static volatile float[] ccValues = new float[51];
        private static volatile bool[] initialized = new bool[51];
        private static volatile bool _shouldStop;

        public static void Init()
        {
            ctrlVals = new ControllerValues();
            midiInputs = InputDevice.InstalledDevices;
            bool connected = false;

            foreach (InputDevice inDevice in midiInputs)
            {
                if (inDevice.Name == "nanoKONTROL2")
                {
                    if (Open(inDevice))
                    {
                        StartInterpThread();
                        connected = true;
                    }
                    break;
                }
            }

            Core.win.midiCtrlButton.Click += MidiCtrlButton_Click;

            if (connected && Properties.Settings.Default.masterIp.Length > 0)
                Connect(Properties.Settings.Default.masterIp);
        }

        public static void SetCtrlVals(ControllerValues ctrlVals_)
        {
            ctrlVals = ctrlVals_;
            initialized = new bool[51];
        }

        public static void Connect(string ip)
        {
            Auto2.UnInitialize();
            Auto2.Initialize(ip, 0);
        }

        private static void MidiCtrlButton_Click(object sender, RoutedEventArgs e)
        {
            dialog = new MidiCtrlDialog(ctrlVals.cmds);
            dialog.Owner = Core.win.mainWindow;
            dialog.setButton.Click += SetButton_Click;
            dialog.Refresh();
            dialog.ShowDialog();
        }

        private static void SetButton_Click(object sender, RoutedEventArgs e)
        {
            SetCtrlVals(dialog.getValues());
            CuelistCtrl.saved = false;
            LogCtrl.Status("MIDI map set.");
        }

        public static void RequestStop()
        {
            _shouldStop = true;
        }

        public static void StartInterpThread()
        {
            interpThread = new Thread(InterLoop);
            interpThread.Start();
        }

        private static void InterLoop()
        {
            try
            {
                float[] lastVal = new float[51];

                while (!_shouldStop)
                {
                    for (int i = 0; i < 51; ++i)
                    {
                        foreach (PbCmdArg pbCmdArg in ctrlVals.pbCmdArgs[i])
                        {
                            float scale = pbCmdArg.max - pbCmdArg.min;
                            float end = pbCmdArg.min + (ccValues[i] * scale);

                            if (lastVal[i] != ccValues[i])
                            {
                                if (!initialized[i])
                                {
                                    pbCmdArg.val = end;
                                    pbCmdArg.step = 0;
                                    initialized[i] = true;

                                    Application.Current.Dispatcher.Invoke(new Action(() =>
                                    {
                                        PbCtrl.ExecuteCmdArg(pbCmdArg, pbCmdArg.val);
                                    }));
                                }
                                else
                                {
                                    float diff = end - pbCmdArg.val;
                                    float step = pbCmdArg.interp;
                                    if (step <= 0) step = 1;
                                    pbCmdArg.step = diff / step;
                                }
                            }

                            if (pbCmdArg.step < 0 && pbCmdArg.val > end || pbCmdArg.step > 0 && pbCmdArg.val < end)
                            {
                                pbCmdArg.val += pbCmdArg.step;

                                float min = Math.Min(pbCmdArg.min, pbCmdArg.max);
                                float max = Math.Max(pbCmdArg.min, pbCmdArg.max);

                                if (pbCmdArg.val < min)
                                    pbCmdArg.val = min;

                                else if (pbCmdArg.val > max)
                                    pbCmdArg.val = max;

                                Application.Current.Dispatcher.Invoke(new Action(() =>
                                {
                                    PbCtrl.ExecuteCmdArg(pbCmdArg, pbCmdArg.val);
                                }));
                            }
                        }

                        if (lastVal[i] != ccValues[i])
                        {
                            lastVal[i] = ccValues[i];

                            if (ccValues[i] <= 0)
                            {
                                foreach (PbCmd pbCmdOff in ctrlVals.pbCmdOff[i])
                                    Application.Current.Dispatcher.Invoke(new Action(() =>
                                    {
                                        PbCtrl.ExecuteCmd(pbCmdOff);
                                    }));

                                foreach (string ccCmdOff in ctrlVals.ccCmdOff[i])
                                    ExecuteCcCmd(ccCmdOff);
                            }
                            else if (ccValues[i] >= 1)
                            {
                                foreach (PbCmd pbCmdOn in ctrlVals.pbCmdOn[i])
                                    Application.Current.Dispatcher.Invoke(new Action(() =>
                                    {
                                        PbCtrl.ExecuteCmd(pbCmdOn);
                                    }));

                                foreach (string ccCmdOn in ctrlVals.ccCmdOn[i])
                                    ExecuteCcCmd(ccCmdOn);
                            }
                        }
                    }
                    Thread.Sleep(40);
                }
            }
            catch
            {
                LogCtrl.ThreadSafeError("MIDI Controller thread crashed!");
            }
        }

        private static void ExecuteCcCmd(string cmd)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                switch (cmd)
                {
                    case "Go":
                        GoCtrl.GoWithThresh();
                        break;
                    case "Back":
                        GoCtrl.GoBack();
                        break;
                    case "Up":
                        CuelistCtrl.SelectPrevCue();
                        break;
                    case "Down":
                        CuelistCtrl.SelectNextCue();
                        break;
                    case "Mute All":
                        MidiInputCtrl.NoteMute(true);
                        MidiInputCtrl.MscMute(true);
                        break;
                    case "Unmute All":
                        MidiInputCtrl.NoteMute(false);
                        MidiInputCtrl.MscMute(false);
                        break;
                    case "Mute Note":
                        MidiInputCtrl.NoteMute(true);
                        break;
                    case "Unmute Note":
                        MidiInputCtrl.NoteMute(false);
                        break;
                    case "Mute MSC":
                        MidiInputCtrl.MscMute(true);
                        break;
                    case "Unmute MSC":
                        MidiInputCtrl.MscMute(false);
                        break;
                    default:
                        string[] buff = cmd.Split(' ');
                        if (buff.Length == 2 && buff[0] == "Script")
                        {
                            int nr;
                            if (int.TryParse(buff[1], out nr))
                                ScriptlistCtrl.ExecuteScript(nr);
                        }
                        break;
                }
            }));
        }

        private static void InputDevice_ControlChange(ControlChangeMessage msg)
        {
            int ctrl = (int)msg.Control;

            if (ctrl >= 16 && ctrl <= 23)
                ctrl -= 8;

            else if (ctrl >= 32 && ctrl <= 39)
                ctrl -= 16;

            else if (ctrl >= 41 && ctrl <= 46)
                ctrl -= 17;

            else if (ctrl >= 48 && ctrl <= 55)
                ctrl -= 18;

            else if (ctrl >= 58 && ctrl <= 62)
                ctrl -= 20;

            else if (ctrl >= 64 && ctrl <= 71)
                ctrl -= 21;

            else if (!(ctrl >= 0 && ctrl <= 7))
                return;

            ccValues[ctrl] = msg.Value / 127f;
        }

        public static void Close()
        {
            RequestStop();

            if (inputDevice != null && inputDevice.IsOpen)
            {
                try
                {
                    Auto2.UnInitialize();
                    inputDevice.StopReceiving();
                    inputDevice.Close();
                    LogCtrl.Status("Close MIDI In: " + inputDevice.Name);
                }
                catch (Exception e)
                {
                    LogCtrl.Error(e.Message);
                }
            }
        }

        public static void LoadMidiMap(DataTable table)
        {
            List<string> cmds = new List<string>();

            foreach (DataRow row in table.Rows)
                cmds.Add(row[1].ToString());

            SetCtrlVals(new ControllerValues(cmds.ToArray()));
        }

        public static DataTable GetMidiMapTable()
        {
            using (DataTable midiTable = new DataTable("MidiMap"))
            {
                midiTable.Columns.Add("Name");
                midiTable.Columns.Add("Commands");

                int x = 0;
                foreach (string name in ControllerLists.GetCtrlNames())
                {
                    midiTable.Rows.Add();
                    midiTable.Rows[x][0] = name;
                    midiTable.Rows[x][1] = string.Join(Environment.NewLine, ctrlVals.cmds[x]);
                    x++;
                }
                return midiTable;
            }
        }

        public static bool Open(InputDevice device)
        {
            try
            {
                inputDevice = device;
                inputDevice.Open();
                inputDevice.StartReceiving(null, true);
                inputDevice.ControlChange += InputDevice_ControlChange;
                LogCtrl.Status("Connect: " + inputDevice.Name);
                return true;
            }
            catch
            {
                LogCtrl.Error("Coudn't connect to " + inputDevice.Name);
                return false;
            }
        }
    }
}


/*
MIDI Map
F1 		= 0
F2 		= 1
F3 		= 2
F4 		= 3
F5 		= 4
F6 		= 5
F7 		= 6
F8		= 7

R1 		= 16 - 8
R2 		= 17 - 9
R3 		= 18 - 10
R4 		= 19 - 11
R5 		= 20 - 12
R6 		= 21 - 13
R7 		= 22 - 14
R8 		= 23 - 15

S1 		= 32 - 16
S2 		= 33 - 17
S3 		= 34 - 18
S4 		= 35 - 19
S5 		= 36 - 20
S6 		= 37 - 21
S7 		= 38 - 22
S8 		= 39 - 23

Start 	= 41 - 24
Stop 	= 42 - 25
RWD 	= 43 - 26
FWD 	= 44 - 27
Rec 	= 45 - 28
Cycle 	= 46 - 29

M1 		= 48 - 30
M2 		= 49 - 31
M3 		= 50 - 32
M4 		= 51 - 33
M5 		= 52 - 34
M6 		= 53 - 35
M7 		= 54 - 36
M8 		= 55 - 37

TLeft   = 58 - 38
TRight  = 59 - 39
Set 	= 60 - 40
MLeft   = 61 - 41
MRight  = 62 - 42

R1		= 64 - 43 
R2		= 65 - 44
R3 		= 66 - 45
R4 		= 67 - 46 
R5 		= 68 - 47
R6		= 69 - 48
R7 		= 70 - 49
R8 		= 71 - 50
*/
