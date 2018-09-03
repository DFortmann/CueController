using CueController3.Controller.Dialog;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CueController3.Controller.Network
{
    class MainBackupCtrl
    {
        private static MyUdpServer server = new MyUdpServer();
        private static MyUdpClient client = new MyUdpClient();
        private static string serverIp;
        private static bool isMain = false;

        public static void Init()
        {
            Core.win.mainModeButton.Click += MainModeButton_Click;
            Core.win.backupModeButton.Click += BackupModeButton_Click;
            Core.win.standaloneModeButton.Click += StandaloneModeButton_Click;

            if (Properties.Settings.Default.backup.Length > 0)
            {
                if (Properties.Settings.Default.backup == "BACKUP") SetBackup();
                else
                {
                    Match match = Regex.Match(Properties.Settings.Default.backup, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                    if (match.Success) SetMain(Properties.Settings.Default.backup);
                }
            }
        }

        private static void MainModeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string ip = InputDialogCtrl.Show("Enter Backup IP:");
            if (ip != null)
            {
                Match match = Regex.Match(ip, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                if (match.Success)
                {
                    Properties.Settings.Default.backup = ip;
                    Properties.Settings.Default.Save();
                    LogCtrl.Warning("Setting to Main. (Backup: " + ip + ")");
                    LogCtrl.Warning("Restart CueController now.");
                }
                else DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Can't set to Main!", "You entered an invalid IP address.");
            }
        }

        private static void BackupModeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Properties.Settings.Default.backup = "BACKUP";
            Properties.Settings.Default.Save();
            LogCtrl.Warning("Setting to Backup Mode.");
            LogCtrl.Warning("Restart CueController now.");
        }

        private static void StandaloneModeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Properties.Settings.Default.backup = "NONE";
            Properties.Settings.Default.Save();
            LogCtrl.Warning("Setting to Standalone Mode.");
            LogCtrl.Warning("Restart CueController now.");
        }

        private static void SetMain(string ip)
        {
            isMain = true;
            serverIp = ip;
            LogCtrl.Status("Set to Main. (Backup: " + ip + ")");
        }

        private static void SetBackup()
        {
            isMain = false;
            if (!server.Init())LogCtrl.Error("Can't set to Backup.");
            else
            {
                server.Start();
                LogCtrl.Status("Set to Backup Mode.");
            }
        }

        public static void Send(string message)
        {
            if (isMain) client.Send(serverIp, message);
        }

        public static void Close()
        {
            server.RequestStop();
        }
    }
}
