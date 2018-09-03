using CueController3.Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace CueController3.Controller.Cues
{
    class CopyCutCtrl
    {
        private static List<Cue> cueBuffer = new List<Cue>();

        public static void Init()
        {
            AddHotKeys();
            Core.win.copyButton.Click += CopyButton_Click;
            Core.win.cutButton.Click += CutButton_Click;
            Core.win.pasteButton.Click += PasteButton_Click;
            Core.win.deleteButton.Click += DeleteButton_Click;
            Core.win.insertButton.Click += InsertButton_Click;
        }

        private static void AddHotKeys()
        {
            Core.win.AddHotKey(Key.I, InsertButton_Click);
            Core.win.AddHotKey(Key.C, CopyButton_Click);
            Core.win.AddHotKey(Key.X, CutButton_Click);
            Core.win.AddHotKey(Key.V, PasteButton_Click);
            Core.win.AddHotKey(Key.D, DeleteButton_Click);
        }

        private static void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (CuelistCtrl.IsIndexInRange(Core.win.cueTable.SelectedIndex))
            {
                cueBuffer.Clear();

                for (int i = 0; i < Core.win.cueTable.SelectedItems.Count; ++i)
                    if (Core.win.cueTable.SelectedItems[i] is Cue)
                    {
                        Cue cue = Core.win.cueTable.SelectedItems[i] as Cue;
                        cueBuffer.Add(cue);
                    }
            }
        }

        private static void CutButton_Click(object sender, RoutedEventArgs e)
        {
            int index = Core.win.cueTable.SelectedIndex;

            
            if (CuelistCtrl.IsIndexInRange(index))
            {
                cueBuffer.Clear();

                for (int i = 0; i < Core.win.cueTable.SelectedItems.Count; ++i)
                {
                    if (Core.win.cueTable.SelectedItems[i] is Cue)
                    {
                        Cue cue = Core.win.cueTable.SelectedItems[i] as Cue;
                        cueBuffer.Add(cue);
                    }
                }

                foreach (Cue c in cueBuffer)
                    CuelistCtrl.cues.Remove(c);

                    CuelistCtrl.SelectIndex(--index);
            }
        }

        private static void PasteButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Cue cue in cueBuffer)
                CuelistCtrl.Insert(cue);
        }

        private static void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int index = Core.win.cueTable.SelectedIndex;
            if (CuelistCtrl.IsIndexInRange(index))
            {
                List<Cue> deleteBuff = new List<Cue>();

                for (int i = 0; i < Core.win.cueTable.SelectedItems.Count; ++i)
                    if (Core.win.cueTable.SelectedItems[i] is Cue)
                    {
                        Cue cue = Core.win.cueTable.SelectedItems[i] as Cue;
                        deleteBuff.Add(cue);
                    }

                foreach (Cue c in deleteBuff)
                    CuelistCtrl.cues.Remove(c);

                CuelistCtrl.SelectIndex(--index);
            }
        }

        private static void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            CuelistCtrl.Insert(null);
        }
    }
}
