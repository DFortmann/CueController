using CueController3.Model;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CueController3.Controller.Cues
{
    class CuelistCtrl
    {
        public static bool editMode = true, saved = true, isManualEditCommit = false;
        public static ObservableCollection<Cue> cues = new ObservableCollection<Cue>();

        public static void Init()
        {
            AddHotKeys();
            Core.win.cueTable.ItemsSource = cues;
            Core.win.cueTable.RowEditEnding += CueTable_RowEditEnding;
            Core.win.cueTable.SelectionChanged += CueTable_SelectionChanged;
            Core.win.editModeMenuItem.Click += EditModeMenuItem_Click;
            Core.win.editButton.Click += EditButton_Click;
            Core.win.renumberButton.Click += RenumberButton_Click;
            Core.win.commentBox.TextChanged += CommentBox_TextChanged;
            Core.win.commentBoxNext.IsReadOnly = true;
            Core.win.commentBoxNext.Focusable = false;
            cues.CollectionChanged += Cues_CollectionChanged;
        }

        private static void CueTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Cue cue = Core.win.cueTable.SelectedItem as Cue;
            if (cue != null) Core.win.commentBox.Text = cue.comment;
            else Core.win.commentBox.Text = "";

            int index = Core.win.cueTable.SelectedIndex + 1;
            if (index >= 0 && index < cues.Count)
                Core.win.commentBoxNext.Text = cues[index].comment;
            else Core.win.commentBoxNext.Text = "";
        }

        private static void AddHotKeys()
        {
            Core.win.AddHotKey(Key.E, EditModeMenuItem_Click);
        }

        private static void CommentBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Cue cue = Core.win.cueTable.SelectedItem as Cue;
            if (cue != null)
            {
                cue.comment = Core.win.commentBox.Text;
                saved = false;
            }
        }

        /// <summary>
        /// This listener is needed for refreshing the first column (trigger symbol) after editing a cue,
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void CueTable_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (Core.win.cueTable.SelectedItem != null && !isManualEditCommit)
            {
                isManualEditCommit = true;
                Core.win.cueTable.CommitEdit();
                Core.win.cueTable.Items.Refresh();
                isManualEditCommit = false;
            }
            saved = false;
        }

        private static void Cues_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (cues.Count > 0 && Core.win.cueTable.SelectedIndex < 0)
                SelectIndex(0);
        }

        private static void EditModeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!editMode) EnableEditMode();
            else DisableEditMode();
        }

        private static void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!editMode) EnableEditMode();
            else DisableEditMode();
        }

        public static void EnableEditMode()
        {
            Core.win.commentBox.IsReadOnly = false;
            Core.win.commentBox.Focusable = true;
            Core.win.cueTable.IsReadOnly = false;
            Core.win.cueTable.SelectionMode = DataGridSelectionMode.Extended;
            Core.win.editButton.Background = new SolidColorBrush(Colors.Red);
            Core.win.goButton.Foreground = Brushes.Red;
            Core.win.goButton.BorderBrush = Brushes.Red;
            editMode = true;
        }

        public static void DisableEditMode()
        {
            Core.win.cueTable.CommitEdit();
            Core.win.commentBox.IsReadOnly = true;
            Core.win.commentBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
            Core.win.commentBox.Focusable = false;
            Core.win.cueTable.IsReadOnly = true;
            Core.win.cueTable.SelectionMode = DataGridSelectionMode.Single;
            Core.win.editButton.Background = new SolidColorBrush(Colors.Transparent);
            Core.win.goButton.Foreground = Brushes.White;
            Core.win.goButton.BorderBrush = Brushes.White;
            editMode = false;
        }

        private static void RenumberButton_Click(object sender, RoutedEventArgs e)
        {
            if (editMode)
            {
                Core.win.cueTable.CommitEdit();
                Core.win.cueTable.IsReadOnly = true;
            }

            for (int i = 0; i < cues.Count; ++i)
                cues[i].Nr = i.ToString();

            Core.win.cueTable.Items.Refresh();

            if (editMode) Core.win.cueTable.IsReadOnly = false;
        }

        public static void Insert(Cue cue)
        {
            Core.win.cueTable.CommitEdit();
            Core.win.cueTable.CommitEdit();
            Core.win.cueTable.Items.Refresh();

            Cue inCue = cue ?? new Cue();
            int index = Core.win.cueTable.SelectedIndex;
            if (index < 0 || index >= cues.Count - 1)
            {
                cues.Add(new Cue()
                {
                    Nr = inCue.Nr,
                    TriggerString = inCue.TriggerString,
                    Description = inCue.Description,
                    SendString = inCue.SendString,
                    comment = inCue.comment
                });
                SelectIndex(cues.Count - 1);
            }
            else
            {
                cues.Insert(++index, new Cue()
                {
                    Nr = inCue.Nr,
                    TriggerString = inCue.TriggerString,
                    Description = inCue.Description,
                    SendString = inCue.SendString,
                    comment = inCue.comment
                });
                SelectIndex(index);
            }
        }

        public static bool SelectIndex(int index)
        {
            if (IsIndexInRange(index))
            {
                Core.win.cueTable.SelectedIndex = index;
                Core.win.cueTable.ScrollIntoView(Core.win.cueTable.SelectedItem);
                return true;
            }
            else return false;
        }

        public static bool IsIndexInRange(int index)
        {
            return (index >= 0 && index < cues.Count) ? true : false;
        }

        public static bool SelectPrevCue()
        {
            return SelectIndex(Core.win.cueTable.SelectedIndex - 1);
        }

        public static bool SelectNextCue()
        {
            return SelectIndex(Core.win.cueTable.SelectedIndex + 1);
        }

        public static DataTable GetCueTable()
        {
            using (DataTable cueTable = new DataTable("Cue"))
            {
                cueTable.Columns.Add("Nr");
                cueTable.Columns.Add("Trigger");
                cueTable.Columns.Add("Send");
                cueTable.Columns.Add("Description");
                cueTable.Columns.Add("Comment");

                int x = 0;
                foreach (Cue cue in cues)
                {
                    cueTable.Rows.Add();
                    cueTable.Rows[x][0] = cue.Nr == null ? "" : cue.Nr;
                    cueTable.Rows[x][1] = cue.TriggerString;
                    cueTable.Rows[x][2] = cue.SendString;
                    cueTable.Rows[x][3] = cue.Description;
                    cueTable.Rows[x][4] = cue.comment;
                    x++;
                }
                return cueTable;
            }
        }

        public static void SetCuesFromTable(DataTable dt)
        {
            cues.Clear();

            foreach (DataRow row in dt.Rows)
            {
                Cue c = new Cue()
                {
                    Nr = row[0].ToString(),
                    TriggerString = row[1].ToString(),
                    Description = row[3].ToString(), //has to be before send, cause: PB check
                    SendString = row[2].ToString(),
                    comment = row[4].ToString()
                };
                cues.Add(c);
            }
        }
    }
}
