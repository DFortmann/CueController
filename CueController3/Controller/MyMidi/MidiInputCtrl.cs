using Midi;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System;
using CueController3.Model;
using System.Windows;
using CueController3.Controller.Dialog;
using CueController3.Controller.Cues;
using CueController3.Controller.Scripts;
using System.Windows.Media;

namespace CueController3.Controller.MyMidi
{
    class MidiInputCtrl
    {
        private static ReadOnlyCollection<InputDevice> midiInputs;
        private static InputDevice inputDevice;
        public static bool noteMute = true, mscMute = true;

        public static void Init()
        {
            midiInputs = InputDevice.InstalledDevices;

            foreach (InputDevice inDevice in midiInputs)
                Core.win.midiInputMenu.Items.Add(new MenuItem()
                {
                    Header = inDevice.Name,
                    IsCheckable = true
                });

            foreach (MenuItem menuItem in Core.win.midiInputMenu.Items)
                menuItem.Checked += MenuItem_Checked;

            if (Properties.Settings.Default.midiInputDeviceName.Length > 0) OpenLastMidiDevice();

            Core.win.saveTriggerCheckbox.IsChecked = true;
        }

        public static void NoteMute(bool b)
        {
            noteMute = b;
            if (noteMute) Core.win.noteMuteButton.Background = new SolidColorBrush(Colors.Red);
            else Core.win.noteMuteButton.Background = new SolidColorBrush(Colors.Transparent);
        }

        public static void MscMute(bool b)
        {
            mscMute = b;
            if (mscMute) Core.win.mscMuteButton.Background = new SolidColorBrush(Colors.Red);
            else Core.win.mscMuteButton.Background = new SolidColorBrush(Colors.Transparent);
        }

        private static void OpenLastMidiDevice()
        {
            foreach (MenuItem item in Core.win.midiInputMenu.Items)
                if (item.Header.ToString() == Properties.Settings.Default.midiInputDeviceName)
                {
                    item.IsChecked = true;
                    return;
                }
        }

        #region "Listeners"
        private static void InputDevice_NoteOn(NoteOnMessage msg)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
        {
            if (msg.Velocity > 0)
            {
                int pitch = (int)msg.Pitch;
                MidiNote note = MidiNote.getMidiNote(pitch);

                if (note != null)
                {
                    if (!noteMute)
                    {
                        if (pitch < 108)
                        {
                            for (int i = 0; i < CuelistCtrl.cues.Count; ++i)
                            {
                                Cue cue = CuelistCtrl.cues[i];
                                if (cue.trigger != null && cue.trigger.type == TriggerType.NOTE && cue.trigger.note.pitch == pitch)
                                {
                                    if (Core.win.saveTriggerCheckbox.IsChecked && Core.win.cueTable.SelectedIndex != i)
                                        LogCtrl.Error("In: " + note.note + " (" + note.pitch + ")");
                                    else
                                    {
                                        LogCtrl.Success("In: " + note.note + " (" + note.pitch + ")");
                                        GoCtrl.Go(i);
                                    }
                                    return;
                                }
                            }
                            LogCtrl.Status("In: " + note.note + " (" + note.pitch + ")");
                        }
                        else
                        {
                            LogCtrl.Status("In: " + note.note + " (" + note.pitch + ")");
                            if (pitch == 108) GoCtrl.GoNextCue();
                            else if (pitch == 109) GoCtrl.GoBack();
                            else if (pitch == 110) CuelistCtrl.SelectPrevCue();
                            else if (pitch == 111) CuelistCtrl.SelectNextCue();
                            else if (pitch >= 112 && pitch <= 121) ScriptlistCtrl.ExecuteScript(note.pitch - 111);
                        }
                    }
                    else LogCtrl.Warning("In: " + note.note + " (" + note.pitch + ")");
                }
            }
        }));
        }

        private static void InputDevice_SysEx(SysExMessage msg)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
        {
            MscCommand mscCommand = MscCommand.getMscCommand(msg.Data);

            if (mscCommand != null)
            {
                if (!mscMute)
                {
                    for (int i = 0; i < CuelistCtrl.cues.Count; ++i)
                    {
                        Cue cue = CuelistCtrl.cues[i];

                        if (cue.trigger != null && cue.trigger.type == TriggerType.MSC && MscCommand.Compare(mscCommand, cue.trigger.mscCmd))
                        {
                            if (Core.win.saveTriggerCheckbox.IsChecked && Core.win.cueTable.SelectedIndex != i)
                                LogCtrl.Error("In: " + mscCommand.message);
                            else
                            {
                                LogCtrl.Success("In: " + mscCommand.message);
                                GoCtrl.Go(i);
                            }
                            return;
                        }
                    }
                    LogCtrl.Status("In: " + mscCommand.message);
                }
                else LogCtrl.Warning("In: " + mscCommand.message);
            }
        }));
        }

        private static void MenuItem_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuItem sendItem = sender as MenuItem;
            sendItem.IsCheckable = false;

            foreach (MenuItem item in Core.win.midiInputMenu.Items)
                if (item != sendItem)
                {
                    item.IsChecked = false;
                    item.IsCheckable = true;
                }

            foreach (InputDevice inDev in midiInputs)
                if (inDev.Name == sendItem.Header.ToString())
                {
                    Open(inDev);
                    return;
                }
        }
        #endregion

        public static void Close()
        {
            if (inputDevice != null && inputDevice.IsOpen)
            {
                try
                {
                    inputDevice.StopReceiving();
                    inputDevice.Close();
                    LogCtrl.Status("Close MIDI In: " + inputDevice.Name);
                }
                catch(Exception e)
                {
                    LogCtrl.Error(e.Message);
                    DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Error", "Coudn't close " + inputDevice.Name);
                }
            }

        }

        public static void Open(InputDevice device)
        {
            Close();

            try
            {
                inputDevice = device;
                inputDevice.Open();
                inputDevice.StartReceiving(null, true);
                inputDevice.SysEx += InputDevice_SysEx;
                inputDevice.NoteOn += InputDevice_NoteOn;
                Properties.Settings.Default.midiInputDeviceName = inputDevice.Name;
                Properties.Settings.Default.Save();
                LogCtrl.Status("Open MIDI In: " + inputDevice.Name);
            }
            catch (Exception e)
            {
                LogCtrl.Error(e.Message);
                DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Error", "Coudn't open " + inputDevice.Name);
            }
        }
    }
}
