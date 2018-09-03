using MahApps.Metro.Controls;

namespace CueController3.View
{
    public enum ScriptAction { APPLY, CLEAR, CANCEL}

    public partial class ScriptDialog : MetroWindow
    {

        public ScriptAction action = ScriptAction.CANCEL;

        public ScriptDialog()
        {
            InitializeComponent();
            okButton.Click += OkButton_Click;
            clearButton.Click += ClearButton_Click;
        }

        private void ClearButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            action = ScriptAction.CLEAR;
            DialogResult = true;
        }

        private void OkButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            action = ScriptAction.APPLY;
            DialogResult = true;
        }
    }
}
