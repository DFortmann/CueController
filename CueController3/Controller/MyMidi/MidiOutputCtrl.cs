using CueController3.Controller.Dialog;
using CueController3.Model;
using Midi;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace CueController3.Controller.MyMidi
{
    class MidiOutputCtrl
    {
        private static ReadOnlyCollection<OutputDevice> midiOutputs;
        private static OutputDevice outputDevice;

        public static void Init()
        {
            midiOutputs = OutputDevice.InstalledDevices;

            foreach (OutputDevice outDevice in midiOutputs)
                Core.win.midiOutputMenu.Items.Add(new MenuItem()
                {
                    Header = outDevice.Name,
                    IsCheckable = true
                });

            foreach (MenuItem menuItem in Core.win.midiOutputMenu.Items)
                menuItem.Checked += MenuItem_Checked;

            OpenLastMidiDevice();
        }

        private static void OpenLastMidiDevice()
        {
            foreach (MenuItem item in Core.win.midiOutputMenu.Items)
                if (item.Header.ToString() == Properties.Settings.Default.midiOutputDeviceName)
                {
                    item.IsChecked = true;
                    return;
                }
        }

        private static void MenuItem_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuItem sendItem = sender as MenuItem;
            sendItem.IsCheckable = false;

            foreach (MenuItem item in Core.win.midiOutputMenu.Items)
                if (item != sendItem)
                {
                    item.IsChecked = false;
                    item.IsCheckable = true;
                }

            foreach (OutputDevice outDev in midiOutputs)
                if (outDev.Name == sendItem.Header.ToString())
                {
                    Open(outDev);
                    return;
                }
        }


        public static void SendMSC(MscCommand mscCommand)
        {
            if (outputDevice != null && outputDevice.IsOpen && !MidiInputCtrl.mscMute)
            {
                try
                {
                    outputDevice.SendSysEx(mscCommand.byteMessage);
                    LogCtrl.Status("Send: " + mscCommand.message);
                }
                catch(Exception e)
                {
                    LogCtrl.Error("Couldn't send MSC command!");
                    LogCtrl.Error(e.ToString());
                }
            }
            else LogCtrl.Warning("Send: " + mscCommand.message);
        }

        public static void SendNote(MidiNote note)
        {
            if (outputDevice != null && outputDevice.IsOpen && !MidiInputCtrl.noteMute)
            {
                try
                {
                    Pitch pitch = (Pitch)note.pitch;
                    outputDevice.SendNoteOn(Channel.Channel1, pitch, 127);
                    outputDevice.SendNoteOn(Channel.Channel1, pitch, 0);
                    LogCtrl.Status("Send: " + note.note + " (" + note.pitch + ")");
                }
                catch(Exception e)
                {
                    LogCtrl.Error("Couldn't send MIDI note!");
                    LogCtrl.Error(e.ToString());
                }
            }
            else LogCtrl.Warning("Send: " + note.note + " (" + note.pitch + ")");
        }

        public static void Close()
        {
            if (outputDevice != null && outputDevice.IsOpen)
            {
                try
                {
                    outputDevice.Close();
                    LogCtrl.Status("Close MIDI Out: " + outputDevice.Name);
                }
                catch (Exception e)
                {
                    LogCtrl.Error(e.Message);
                    DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Error", "Coudn't close " + outputDevice.Name);
                }
            }

        }

        public static void Open(OutputDevice device)
        {
            Close();
            try
            {
                outputDevice = device;
                outputDevice.Open();
                Properties.Settings.Default.midiOutputDeviceName = outputDevice.Name;
                Properties.Settings.Default.Save();
                LogCtrl.Status("Open MIDI Out: " + outputDevice.Name);
            }
            catch (Exception e)
            {
                LogCtrl.Error(e.Message);
                DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Error", "Coudn't open " + outputDevice.Name);
            }
        }
    }
}
