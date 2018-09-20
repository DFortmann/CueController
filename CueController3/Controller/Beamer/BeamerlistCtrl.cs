using CueController3.Controller.Cues;
using CueController3.Controller.Dialog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace CueController3.Controller.Beamer
{
    class BeamerlistCtrl
    {
        public static List<BeamerCtrl> beamers = new List<BeamerCtrl>();

        public static void Init()
        {
            Core.win.addBeamerButton.Click += AddBeamerButton_Click;
        }

        public static void LoadBeamers(DataTable table)
        {
            try
            {
                beamers.Clear();

                foreach (DataRow row in table.Rows)
                {
                    int id = int.Parse(row[0].ToString());
                    string ip = row[1].ToString();

                    beamers.Add(new BeamerCtrl(id, ip));
                }
                beamers.Sort((x, y) => x.id.CompareTo(y.id));
                RefreshBeamerMenus();
            }
            catch (Exception e)
            {
                LogCtrl.Error("Couldn't load beamers.");
                LogCtrl.Error(e.ToString());
            }
        }

        private static void AddBeamerButton_Click(object sender, RoutedEventArgs e)
        {
            string s = InputDialogCtrl.Show("Enter: ID IP");

            if (s != null)
            {
                string[] a = s.Split(' ');
                if (a.Length >= 2)
                {
                    int id;
                    if (!int.TryParse(a[0], out id)) DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Can't add beamer!", "Invalid ID.");
                    else
                    {
                        Match match = Regex.Match(a[1], @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$");
                        if (!match.Success) DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Can't add beamer!", "Invalid IP.");
                        else AddProjector(id, a[1]);
                    }
                    CuelistCtrl.saved = false;
                }
                else DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Can't add beamer!", "Invalid text input.");
            }
        }

        private static void RemoveBeamer_Click(object sender, RoutedEventArgs e)
        {
            MenuItem sendItem = sender as MenuItem;
            int id;

            if (int.TryParse(sendItem.Header.ToString().Split(' ')[0], out id))
            {
                foreach (BeamerCtrl b in beamers)
                {
                    if (b.id == id)
                    {
                        beamers.Remove(b);
                        Core.win.removeBeamerMenu.Items.Remove(sendItem);

                        foreach (MenuItem mi in Core.win.openShutterMenu.Items)
                            if (mi.Header.ToString().StartsWith(id.ToString()))
                            {
                                Core.win.openShutterMenu.Items.Remove(mi);
                                break;
                            }

                        foreach (MenuItem mi in Core.win.closeShutterMenu.Items)
                            if (mi.Header.ToString().StartsWith(id.ToString()))
                            {
                                Core.win.closeShutterMenu.Items.Remove(mi);
                                break;
                            }

                        break;
                    }
                }
                CuelistCtrl.saved = false;
                LogCtrl.Status("Removed Beamer " + id);
            }
        }

        private static void ShutterClose_Click(object sender, RoutedEventArgs e)
        {
            MenuItem sendItem = sender as MenuItem;
            int id;
            if (int.TryParse(sendItem.Header.ToString().Split(' ')[0], out id))
                Shutter(id, false);
        }

        private static void ShutterOpen_Click(object sender, RoutedEventArgs e)
        {
            MenuItem sendItem = sender as MenuItem;
            int id;
            if (int.TryParse(sendItem.Header.ToString().Split(' ')[0], out id))
                Shutter(id, true);
        }

        public static void AddProjector(int id, string ip)
        {
            foreach (BeamerCtrl b in beamers)
                if (b.id == id)
                {
                    DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Can't add beamer.", "Beamer ID already exists!");
                    return;
                }

            beamers.Add(new BeamerCtrl(id, ip));
            beamers.Sort((x, y) => x.id.CompareTo(y.id));
            RefreshBeamerMenus();

            LogCtrl.Status("Added Beamer " + id + " " + ip);
        }

        public static void Shutter(int id, bool open)
        {
            foreach (BeamerCtrl b in beamers)
                if (b.id == id)
                {
                    b.Shutter(open);
                    return;
                }
            LogCtrl.Error("Beamer " + id + " doesn't exist.");
        }

        public static void RefreshBeamerMenus()
        {
            Core.win.openShutterMenu.Items.Clear();
            Core.win.closeShutterMenu.Items.Clear();
            Core.win.removeBeamerMenu.Items.Clear();

            foreach (BeamerCtrl b in beamers)
            {
                MenuItem mi = new MenuItem();
                mi.Header = b.id + " " + b.ip;
                mi.Click += ShutterOpen_Click;
                Core.win.openShutterMenu.Items.Add(mi);

                mi = new MenuItem();
                mi.Header = b.id + " " + b.ip;
                mi.Click += ShutterClose_Click;
                Core.win.closeShutterMenu.Items.Add(mi);

                mi = new MenuItem();
                mi.Header = b.id + " " + b.ip;
                mi.Click += RemoveBeamer_Click;
                Core.win.removeBeamerMenu.Items.Add(mi);
            }
        }

        public static DataTable GetBeamerTable()
        {
            using (DataTable beamerTable = new DataTable("Beamer"))
            {
                beamerTable.Columns.Add("ID");
                beamerTable.Columns.Add("IP");

                foreach (BeamerCtrl beamer in beamers)
                    beamerTable.Rows.Add(beamer.id, beamer.ip);

                return beamerTable;
            }
        }
    }
}
