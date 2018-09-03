using CueController3.Controller.Cues;
using CueController3.Controller.Dialog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace CueController3.Controller.Network
{
    class OscListCtrl
    {
        public static List<OscTarget> oscTargets = new List<OscTarget>();

        public static void Init()
        {
            Core.win.addOscButton.Click += AddOscButton_Click;
        }

        public static void LoadOscTargets(DataTable table)
        {
            try
            {
                oscTargets.Clear();

                foreach(DataRow row in table.Rows)
                {
                    int id = int.Parse(row[0].ToString());
                    IPAddress ip = IPAddress.Parse(row[1].ToString());
                    int port = int.Parse(row[2].ToString());
                    oscTargets.Add(new OscTarget(id, ip, port));
                }
                oscTargets.Sort((x, y) => x.id.CompareTo(y.id));
                RefreshOscMenu();
            }catch(Exception e)
            {
                LogCtrl.Error("Couldn't load OSC targets.");
                LogCtrl.Error(e.ToString());

            }
        }

        public static DataTable GetOscTargetTable()
        {
            using (DataTable oscTargetTable = new DataTable("OscTargets"))
            {
                oscTargetTable.Columns.Add("ID");
                oscTargetTable.Columns.Add("IP");
                oscTargetTable.Columns.Add("Port");

                foreach (OscTarget oscTarget in oscTargets)
                    oscTargetTable.Rows.Add(oscTarget.id, oscTarget.ip, oscTarget.port);

                return oscTargetTable;
            }
        }

        private static void AddOscButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string s = InputDialogCtrl.Show("Enter ID + IP + Port");

            if (s != null)
            {
                string[] buff = s.Split(' ');
                if (buff.Length >= 3)
                {
                    int id;
                    if (!int.TryParse(buff[0], out id)) DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Can't add OSC target!", "Invalid ID.");
                    else
                    {
                        Match match = Regex.Match(buff[1], @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$");
                        if (!match.Success) DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Can't add OSC target!", "Invalid IP.");
                        else
                        {
                            IPAddress ip = IPAddress.Parse(buff[1]);
                            int port;
                            if (!int.TryParse(buff[2], out port)) DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Can't add OSC target!", "Invalid Port.");
                            else AddOscTarget(id, ip, port);
                        }
                        CuelistCtrl.saved = false;
                    }
                }
                else DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Wrong Format!", "Enter ID (chosen by you), IP and Port.");
            }
        }

        private static void AddOscTarget(int id, IPAddress ip, int port)
        {
            foreach (OscTarget oscTarget in oscTargets)
                if (oscTarget.id == id)
                {
                    DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Can't add OSC target!", "ID alread exists.");
                    return;
                }
            oscTargets.Add(new OscTarget(id, ip, port));
            oscTargets.Sort((x, y) => x.id.CompareTo(y.id));
            RefreshOscMenu();

            LogCtrl.Status("Added OSC Target.");
        }

        public static void RefreshOscMenu()
        {
            Core.win.removeOscMenu.Items.Clear();

            foreach (OscTarget target in oscTargets)
            {
                MenuItem mi = new MenuItem();
                mi.Header = target.id + " " + target.ip + ":" + target.port;
                mi.Click += RemoveOscTarget_Click;
                Core.win.removeOscMenu.Items.Add(mi);
            }
        }

        private static void RemoveOscTarget_Click(object sender, RoutedEventArgs e)
        {
            MenuItem sendItem = sender as MenuItem;
            int id;

            if (int.TryParse(sendItem.Header.ToString().Split(' ')[0], out id))
            {
                foreach (OscTarget target in oscTargets)
                {
                    if (target.id == id)
                    {

                        oscTargets.Remove(target);
                        Core.win.removeOscMenu.Items.Remove(sendItem);
                        break;
                    }
                }
                CuelistCtrl.saved = false;
                LogCtrl.Status("Removed OSC Target.");
            }
        }

       public class OscTarget
        {
            public int id, port;
            public IPAddress ip;

            public OscTarget(int id, IPAddress ip, int port)
            {
                this.id = id;
                this.ip = ip;

                this.port = port;
            }
        }
    }
}