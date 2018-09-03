using System.Windows;
using System.Windows.Controls;

namespace CueController3.Controller.Files
{
    class RecentFilesCtrl
    {
        public static void Init()
        {
            Refresh();
        }

        public static void Add(string filename)
        {
            if (Properties.Settings.Default.recentFiles.Count > 9)
                Properties.Settings.Default.recentFiles.RemoveAt(9);

            foreach (string s in Properties.Settings.Default.recentFiles)
                if (s == filename)
                {
                    Properties.Settings.Default.recentFiles.Remove(s);
                    break;
                }

            Properties.Settings.Default.recentFiles.Insert(0, filename);
            Properties.Settings.Default.Save();
            Refresh();
        }

        public static void Refresh()
        {
            Core.win.recentFilesMenu.Items.Clear();

            if (Properties.Settings.Default.recentFiles != null)
            {
                foreach (string f in Properties.Settings.Default.recentFiles)
                    Core.win.recentFilesMenu.Items.Add(new MenuItem()
                    {
                        Header = f,
                    });

                foreach (MenuItem item in Core.win.recentFilesMenu.Items)
                    item.Click += RecentFile_Click;
            }
            else Properties.Settings.Default.recentFiles = new System.Collections.Specialized.StringCollection();
        }

        private static void RecentFile_Click(object sender, RoutedEventArgs e)
        {
            ReadWriteCtrl.Read((sender as MenuItem).Header.ToString());
        }
    }
}
