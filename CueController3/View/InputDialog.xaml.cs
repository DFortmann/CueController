using MahApps.Metro.Controls;

namespace CueController3.View
{
    public partial class InputDialog : MetroWindow
    {
        public InputDialog()
        {
            InitializeComponent();
            okButton.Click += OkButton_Click;
        }

        private void OkButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
