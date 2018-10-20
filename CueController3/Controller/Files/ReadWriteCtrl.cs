using CueController3.Controller.Beamer;
using CueController3.Controller.Cues;
using CueController3.Controller.Dialog;
using CueController3.Controller.MyMidi;
using CueController3.Controller.Scripts;
using CueController3.Model;
using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace CueController3.Controller.Files
{
    class ReadWriteCtrl
    {
        private static string path;

        public static void Init()
        {
            Core.win.newButton.Click += NewButton_Click;
            Core.win.openButton.Click += OpenButton_Click;
            Core.win.saveButton.Click += SaveButton_Click;
            Core.win.saveAsButton.Click += SaveAsButton_Click;
            AddHotKeys();
        }

        private static void AddHotKeys()
        {
            Core.win.AddHotKey(Key.N, NewButton_Click);
            Core.win.AddHotKey(Key.O, OpenButton_Click);
            Core.win.AddHotKey(Key.S, SaveButton_Click);
        }

        private static void NewButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CuelistCtrl.saved)
            {
                bool? result = DialogCtrl.Show(DialogType.QUESTION, OptionType.YESNO, "Create new Cuelist", "Do you want to save the current cuelist?");
                if (result == true)
                {
                    if (Write(false)) ClearCuelist();
                }
                else ClearCuelist();
            }
            else ClearCuelist();
        }

        private static void ClearCuelist()
        {
            CuelistCtrl.cues.Clear();
            CuelistCtrl.saved = true;
            ScriptlistCtrl.ClearScripts();
            BeamerlistCtrl.beamers.Clear();
            BeamerlistCtrl.RefreshBeamerMenus();
            MidiController.SetCtrlVals(new ControllerValues());
            SetPath(null);
        }

        private static void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            Read(null);
        }

        private static void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Write(true);
        }

        private static void SaveAsButton_Click(object sender, RoutedEventArgs e)
        {
            Write(false);
        }

        public static void Read(string filename)
        {
            if (!CuelistCtrl.saved)
            {
                bool? result = DialogCtrl.Show(DialogType.QUESTION, OptionType.YESNO, "Open Cuelist", "Do you want to save the current cuelist?");
                if (result == true)
                {
                    if (!Write(false)) return;
                }
            }

            if (filename == null)
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.Filter = "CC Files (*.cc)|*.cc";
                bool? result = dlg.ShowDialog();

                if (result == true) filename = dlg.FileName;
                else return;
            }

            using (DataSet dataSet = new DataSet("Cuelist"))
            {
                try { dataSet.ReadXml(filename); }
                catch (Exception e)
                {
                    LogCtrl.Error(e.Message);
                    DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Error", "Can't open file!");
                    return;
                }
                DataTableCollection collection = dataSet.Tables;

                ClearCuelist();

                for (int i = 0; i < collection.Count; i++)
                {
                    DataTable table = collection[i];
                    switch (table.TableName)
                    {
                        case "Settings":
                            SetSettingsFromTable(table);
                            break;
                        case "Script":
                            ScriptlistCtrl.LoadScripts(table);
                            break;
                        case "Beamer":
                            BeamerlistCtrl.LoadBeamers(table);
                            break;
                        case "MidiMap":
                            MidiController.LoadMidiMap(table);
                            break;
                        default:
                            CuelistCtrl.SetCuesFromTable(table);
                            break;
                    }
                }
            }

            CuelistCtrl.saved = true;
            SetPath(filename);
            CuelistCtrl.DisableEditMode();
            RecentFilesCtrl.Add(filename);
            LogCtrl.Status("Open file: " + filename);
        }

        public static bool Write(bool overwrite)
        {
            string filename;

            if (overwrite && path != null) filename = path;
            else
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.Filter = "CC Files (*.cc)|*.cc";
                bool? result = dlg.ShowDialog();
                if (result == false) return false;
                filename = dlg.FileName;
            }

            string p = Path.ChangeExtension(filename, ".cc");

            using (DataSet dataSet = new DataSet("Cuelist"))
            {
                dataSet.Tables.Add(GetSettingsTable());
                dataSet.Tables.Add(CuelistCtrl.GetCueTable());
                dataSet.Tables.Add(ScriptlistCtrl.GetScriptTable());
                dataSet.Tables.Add(BeamerlistCtrl.GetBeamerTable());
                dataSet.Tables.Add(MidiController.GetMidiMapTable());

                try { dataSet.WriteXml(p); }
                catch (Exception e)
                {
                    LogCtrl.Error(e.Message);
                    DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Error", "Couldn't write file!");
                    return false;
                }
            }

            SetPath(p);
            CuelistCtrl.saved = true;
            RecentFilesCtrl.Add(p);
            LogCtrl.Status("Saved file: " + p);
            return true;
        }

        public static void SetPath(string _path)
        {
            if (_path != null)
            {
                path = _path;
                Core.win.mainWindow.Title = "CueController - " + Path.GetFileNameWithoutExtension(path);
            }
            else
            {
                path = null;
                Core.win.mainWindow.Title = "CueController";
            }
        }

        private static DataTable GetSettingsTable()
        {
            using (DataTable settingsTable = new DataTable("Settings"))
            {
                settingsTable.Columns.Add("SaveTrigger");
                settingsTable.Rows.Add(Core.win.saveTriggerCheckbox.IsChecked);
                return settingsTable;
            }
        }

        public static void SetSettingsFromTable(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                bool b;
                if (Boolean.TryParse(row[0].ToString(), out b))
                {
                    Core.win.saveTriggerCheckbox.IsChecked = b;
                }
                else
                {
                    LogCtrl.Error("Couldn't read Save Trigger settings!");
                }
            }
           
        }
    }
}
