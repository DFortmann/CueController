using CueController3.Controller.Dialog;
using System.Collections.Generic;
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

            if (Properties.Settings.Default.oscTargets == null)
                Properties.Settings.Default.oscTargets = new System.Collections.Specialized.StringCollection();

            foreach (string oscTarget in Properties.Settings.Default.oscTargets)
            {
                string[] target = oscTarget.Split(' ');
                oscTargets.Add(new OscTarget(target[0], target[1], IPAddress.Parse(target[2]), int.Parse(target[3]), oscTarget));
            }
            RefreshOscMenu();
        }

        private static void AddOscButton_Click(object sender, RoutedEventArgs e)
        {
            string s = InputDialogCtrl.Show("Enter: Keyword Prefix IP Port", 300);

            if (s != null)
            {
                string[] buff = s.Split(' ');
                if (buff.Length >= 4)
                {
                    if (!buff[1].StartsWith("/")) DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Wrong prefix format.", "OSC addresses have to start with /");
                    else if (buff[1].EndsWith("/")) DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Wrong prefix format.", "Prefix cannot end with /");
                    else
                    {
                        Match match = Regex.Match(buff[2], @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$");
                        if (!match.Success) DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Can't add OSC target!", "Invalid IP.");
                        else
                        {
                            if (!int.TryParse(buff[3], out int port)) DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Can't add OSC target!", "Invalid Port.");
                            else AddOscTarget(buff[0], buff[1], IPAddress.Parse(buff[2]), port, s);
                        }
                    }
                }
                else DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Wrong Format!", "Enter ID (chosen by you), IP and Port.");
            }
        }

        private static void AddOscTarget(string keyword, string prefix, IPAddress ip, int port, string wholeString)
        {
            foreach (OscTarget oscTarget in oscTargets)
                if (oscTarget.keyword == keyword)
                {
                    DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Can't add OSC target!", "Keyword was already assigned.");
                    return;
                }

            oscTargets.Add(new OscTarget(keyword, prefix, ip, port, wholeString));

            Properties.Settings.Default.oscTargets.Add(wholeString);
            Properties.Settings.Default.Save();

            oscTargets.Sort((x, y) => x.keyword.CompareTo(y.keyword));
            RefreshOscMenu();

            LogCtrl.Status("Added OSC Target.");
        }

        public static void RefreshOscMenu()
        {
            Core.win.removeOscMenu.Items.Clear();

            foreach (OscTarget target in oscTargets)
            {
                MenuItem mi = new MenuItem();
                mi.Header = target.keyword + " " + target.prefix + " " + target.ip + ":" + target.port;
                mi.Click += RemoveOscTarget_Click;
                Core.win.removeOscMenu.Items.Add(mi);
            }
        }

        private static void RemoveOscTarget_Click(object sender, RoutedEventArgs e)
        {
            MenuItem sendItem = sender as MenuItem;
            string keyword = sendItem.Header.ToString().Split(' ')[0];
            foreach (OscTarget target in oscTargets)
            {
                if (target.keyword == keyword)
                {
                    oscTargets.Remove(target);
                    Properties.Settings.Default.oscTargets.Remove(target.wholeString);
                    Properties.Settings.Default.Save();
                    Core.win.removeOscMenu.Items.Remove(sendItem);
                    break;
                }
            }

            LogCtrl.Status("Removed OSC Target.");
        }
    }

    public class OscTarget
    {
        public string keyword, prefix, wholeString;
        public IPAddress ip;
        public int port;

        public OscTarget(string keyword, string prefix, IPAddress ip, int port, string wholeString)
        {
            this.keyword = keyword;
            this.prefix = prefix;
            this.ip = ip;
            this.port = port;
            this.wholeString = wholeString;
        }
    }
}
