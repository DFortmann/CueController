using MahApps.Metro.Controls;

namespace CueController3.View
{
    public partial class InputDialog : MetroWindow
    {
        public InputDialog(int width)
        {
            InitializeComponent();
            if(width > 0) inputDialog.Width = width;
            okButton.Click += OkButton_Click;
        }

        private void OkButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
