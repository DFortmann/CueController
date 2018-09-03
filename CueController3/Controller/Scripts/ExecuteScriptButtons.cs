using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CueController3.Controller.Scripts
{
    class ExecuteScriptButtons
    {

        public static void Setup(MenuItem[] executeItems)
        {
            Core.win.AddHotKey(Key.D1, ExecuteScriptButton1_Click);
            Core.win.AddHotKey(Key.D2, ExecuteScriptButton2_Click);
            Core.win.AddHotKey(Key.D3, ExecuteScriptButton3_Click);
            Core.win.AddHotKey(Key.D4, ExecuteScriptButton4_Click);
            Core.win.AddHotKey(Key.D5, ExecuteScriptButton5_Click);
            Core.win.AddHotKey(Key.D6, ExecuteScriptButton6_Click);
            Core.win.AddHotKey(Key.D7, ExecuteScriptButton7_Click);
            Core.win.AddHotKey(Key.D8, ExecuteScriptButton8_Click);
            Core.win.AddHotKey(Key.D9, ExecuteScriptButton9_Click);
            Core.win.AddHotKey(Key.D0, ExecuteScriptButton10_Click);

            executeItems[0] = Core.win.executeScriptButton1;
            executeItems[1] = Core.win.executeScriptButton2;
            executeItems[2] = Core.win.executeScriptButton3;
            executeItems[3] = Core.win.executeScriptButton4;
            executeItems[4] = Core.win.executeScriptButton5;
            executeItems[5] = Core.win.executeScriptButton6;
            executeItems[6] = Core.win.executeScriptButton7;
            executeItems[7] = Core.win.executeScriptButton8;
            executeItems[8] = Core.win.executeScriptButton9;
            executeItems[9] = Core.win.executeScriptButton10;

            executeItems[0].Click += ExecuteScriptButton1_Click;
            executeItems[1].Click += ExecuteScriptButton2_Click;
            executeItems[2].Click += ExecuteScriptButton3_Click;
            executeItems[3].Click += ExecuteScriptButton4_Click;
            executeItems[4].Click += ExecuteScriptButton5_Click;
            executeItems[5].Click += ExecuteScriptButton6_Click;
            executeItems[6].Click += ExecuteScriptButton7_Click;
            executeItems[7].Click += ExecuteScriptButton8_Click;
            executeItems[8].Click += ExecuteScriptButton9_Click;
            executeItems[9].Click += ExecuteScriptButton10_Click;
        }

        private static void ExecuteScriptButton1_Click(object sender, RoutedEventArgs e)
        {
            ScriptlistCtrl.ExecuteScript(1);
        }

        private static void ExecuteScriptButton2_Click(object sender, RoutedEventArgs e)
        {
            ScriptlistCtrl.ExecuteScript(2);
        }

        private static void ExecuteScriptButton3_Click(object sender, RoutedEventArgs e)
        {
            ScriptlistCtrl.ExecuteScript(3);
        }

        private static void ExecuteScriptButton4_Click(object sender, RoutedEventArgs e)
        {
            ScriptlistCtrl.ExecuteScript(4);
        }

        private static void ExecuteScriptButton5_Click(object sender, RoutedEventArgs e)
        {
            ScriptlistCtrl.ExecuteScript(5);
        }

        private static void ExecuteScriptButton6_Click(object sender, RoutedEventArgs e)
        {
            ScriptlistCtrl.ExecuteScript(6);
        }

        private static void ExecuteScriptButton7_Click(object sender, RoutedEventArgs e)
        {
            ScriptlistCtrl.ExecuteScript(7);
        }

        private static void ExecuteScriptButton8_Click(object sender, RoutedEventArgs e)
        {
            ScriptlistCtrl.ExecuteScript(8);
        }

        private static void ExecuteScriptButton9_Click(object sender, RoutedEventArgs e)
        {
            ScriptlistCtrl.ExecuteScript(9);
        }

        private static void ExecuteScriptButton10_Click(object sender, RoutedEventArgs e)
        {
            ScriptlistCtrl.ExecuteScript(10);
        }
    }
}
