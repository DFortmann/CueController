using CueController3.Controller.Cues;
using CueController3.Model;
using CueController3.View;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace CueController3.Controller.Scripts
{
    class ScriptlistCtrl
    {
        private static ScriptDialog dialog;
        public static Script[] scripts = new Script[10];
        private static MenuItem[] editItems = new MenuItem[10];
        public static MenuItem[] executeItems = new MenuItem[10];

        public static void Init()
        {
            SetupEditButtons();
            ExecuteScriptButtons.Setup(executeItems);
        }

        private static void SetupEditButtons()
        {
            editItems[0] = Core.win.editScriptButton1;
            editItems[1] = Core.win.editScriptButton2;
            editItems[2] = Core.win.editScriptButton3;
            editItems[3] = Core.win.editScriptButton4;
            editItems[4] = Core.win.editScriptButton5;
            editItems[5] = Core.win.editScriptButton6;
            editItems[6] = Core.win.editScriptButton7;
            editItems[7] = Core.win.editScriptButton8;
            editItems[8] = Core.win.editScriptButton9;
            editItems[9] = Core.win.editScriptButton10;

            for (int i = 0; i < 10; ++i)
                editItems[i].Click += EditScriptButton_Click;
        }

        private static void EditScriptButton_Click(object sender, RoutedEventArgs e)
        {
            int scriptNr = int.Parse((sender as MenuItem).Header.ToString().Split(' ')[0]);
            EditScript(scriptNr);
        }

        private static void EditScript(int nr)
        {
            string[] buff = Show(nr);

            if (buff != null)
            {
                if (buff.Length == 2)
                {
                    Script script = CreateScript(buff[0], buff[1]);

                    if (script != null && script.cues.Count > 0)
                    {
                        scripts[nr - 1] = script;
                        editItems[nr - 1].Header = nr + " " + script.title;
                        executeItems[nr - 1].Header = nr + " " + script.title;

                        foreach (Cue cue in script.cues)
                        {
                            if (cue.SendString == "PB") LogCtrl.Status("Added: " + cue.SendString + " " + cue.Description);
                            else LogCtrl.Status("Added: " + cue.SendString);
                        }
                    }
                }
                else if (buff.Length == 1)
                {
                    scripts[nr - 1] = null;
                    editItems[nr - 1].Header = nr.ToString();
                    executeItems[nr - 1].Header = nr.ToString();
                    LogCtrl.Status("Script " + nr + " cleared.");
                }
                CuelistCtrl.saved = false;
            }
        }

        private static string[] Show(int scriptNr)
        {
            dialog = new ScriptDialog();
            dialog.Owner = Core.win.mainWindow;
            dialog.scriptDialog.Title = "Edit Script " + scriptNr;
            if (scripts[scriptNr - 1] != null)
            {
                dialog.titleBox.Text = scripts[scriptNr - 1].title;
                dialog.textBox.Text = scripts[scriptNr - 1].scriptString;
            }
            dialog.titleBox.Focus();

            if (dialog.ShowDialog() == true)
            {
                if (dialog.action == ScriptAction.APPLY) return new string[] { dialog.titleBox.Text.Trim(), dialog.textBox.Text.Trim() };
                else if (dialog.action == ScriptAction.CLEAR) return new string[] { "Cleared" };
            }
            return null;
        }

        private static Script CreateScript(string title, string scriptString)
        {
            string[] lines = scriptString.Split('\n');
            Script script = new Script(title, lines, scriptString);
            if (script.cues.Count == lines.Length) LogCtrl.Status("Script created successfully.");
            else LogCtrl.Warning("Not all commands could be added to the script!");
            return script;
        }

        public static DataTable GetScriptTable()
        {
            using (DataTable scriptTable = new DataTable("Script"))
            {
                scriptTable.Columns.Add("Name");
                scriptTable.Columns.Add("Send");

                for (int i = 0; i < 10; ++i)
                {
                    Script script = scripts[i];
                    if (script != null)
                        scriptTable.Rows.Add(script.title, script.scriptString);
                    else
                        scriptTable.Rows.Add();
                }
                return scriptTable;
            }
        }

        public static void LoadScripts(DataTable dataTable)
        {
            try
            {
                int x = 0;
                foreach (DataRow row in dataTable.Rows)
                {
                    if(row.ItemArray.Length == 2)
                    {
                        string title = row[0].ToString();
                        string body = row[1].ToString();
                        string[] lines = body.Split('\n');
                        Script script = new Script(title, lines, body);
                        editItems[x].Header = (x + 1) + " " + script.title;
                        executeItems[x].Header = (x + 1) + " " + script.title;
                        scripts[x] = script;
                    }
                    x++;
                }
            }
            catch (Exception e)
            {
                LogCtrl.Error("Couldn't load Scripts.");
                LogCtrl.Error(e.ToString());
            }
        }

        public static void ClearScripts()
        {
            for (int i = 0; i < 10; ++i)
            {
                editItems[i].Header = (i + 1) + "";
                executeItems[i].Header = (i + 1) + "";
            }
            scripts = new Script[10];
        }

        public static void ExecuteScript(int nr)
        {
            if (scripts[nr - 1] != null)
            {
                LogCtrl.Status("Execute Script " + nr + ": " + scripts[nr - 1].title);
                foreach (Cue cue in scripts[nr - 1].cues)
                    GoCtrl.ExecuteCueSend(cue);
            }
            else LogCtrl.Error("Script doesn't exist.");
        }
    }
}
