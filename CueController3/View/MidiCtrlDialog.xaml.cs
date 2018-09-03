using CueController3.Model;
using MahApps.Metro.Controls;

namespace CueController3.View
{
    public partial class MidiCtrlDialog : MetroWindow
    {
        private string[] cmds;
       
        public MidiCtrlDialog(string[] _cmds)
        {
            InitializeComponent();
            cmds = _cmds;

            ctrlChoice.ItemsSource = ControllerLists.GetCtrlNames();
            ctrlChoice.SelectedIndex = 0;
            ctrlChoice.SelectionChanged += CtrlBox_SelectionChanged;

            addCmd.ItemsSource = ControllerLists.GetCmdList();
            addCmd.SelectionChanged += AddCmd_SelectionChanged;

            cmdBox.TextChanged += CmdBox_TextChanged;
        }

        private void CmdBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            cmds[GetIndex()] = cmdBox.Text;
        }

        
        private void AddCmd_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string cmd = addCmd.SelectedValue.ToString();
            cmdBox.AppendText(cmd);
            if (addCmd.SelectedIndex != 0)
                addCmd.SelectedIndex = 0;
        }
        
        private void CtrlBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Refresh();
        }

        public void Refresh()
        {
            int index = GetIndex();
            cmdBox.Text = cmds[index];
        }

        public ControllerValues getValues()
        {
           return new ControllerValues(cmds);
        }

        private int GetIndex()
        {
            int index = ctrlChoice.SelectedIndex;

            if (index >= 16 && index <= 21)
                index += 8;

            else if (index >= 22 && index <= 26)
                index += 16;

            else if (index >= 27 && index <= 34)
                index -= 11;

            else if (index >= 35 && index <= 42)
                index -= 5;

            return index;
        }
    }
}

