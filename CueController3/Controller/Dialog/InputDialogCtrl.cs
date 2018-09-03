using CueController3.View;

namespace CueController3.Controller.Dialog
{
    class InputDialogCtrl
    {
        private static InputDialog dialog;

        public static string Show(string header)
        {
            dialog = new InputDialog();
            dialog.Owner = Core.win.mainWindow;
            dialog.inputDialog.Title = header;
            dialog.textBox.Focus();

            if (dialog.ShowDialog() == true)
                return dialog.textBox.Text;
            else return null;
        }
    }
}
