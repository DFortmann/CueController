using CueController3.Controller.Dialog;
using CueController3.Model;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;

namespace CueController3.Controller.Network
{
    class MatrixCtrl
    {
        private static ASCIIEncoding asen = new ASCIIEncoding();
        private static string ip;

        public static void Init()
        {
            Core.win.matrixIpButton.Click += MatrixIpButton_Click;
            Core.win.matrixPresetButton.Click += MatrixPresetButton_Click;

            if (Properties.Settings.Default.matrixIp.Length > 0)
                SetIp(Properties.Settings.Default.matrixIp);
        }

        private static void MatrixPresetButton_Click(object sender, RoutedEventArgs e)
        {
            string s = InputDialogCtrl.Show("Enter preset number");
            if (s != null)
            {
                int preset;
                if (int.TryParse(s, out preset))
                {
                    MatrixCmd cmdBuff = MatrixCmd.getMatrixCmd("P"+preset);
                    SendMatrixCmd(cmdBuff);
                }
                else DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Can't load preset!", "You entered an invalid preset number.");
            }
        }

        private static void MatrixIpButton_Click(object sender, RoutedEventArgs e)
        {
            string s = InputDialogCtrl.Show("Enter Master IP");
            if (s != null)
            {
                Match match = Regex.Match(s, @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$");
                if (match.Success) SetIp(s);
                else DialogCtrl.Show(DialogType.ERROR, OptionType.OKCANCEL, "Can't set Matrix IP!", "You entered an invalid IP address.");
            }
        }

        private static void SetIp(string _ip)
        {
            ip = _ip;
            Properties.Settings.Default.matrixIp = ip;
            Properties.Settings.Default.Save();
            LogCtrl.Status("Set Matrix IP: " + ip);
        }

        public static void SendMatrixCmd(MatrixCmd matrixCmd)
        {
            LogCtrl.Status(matrixCmd.description);
            Thread thread = new Thread(() => ExecuteMatrixCmd(matrixCmd));
            thread.Start();
        }

        private static void ExecuteMatrixCmd(MatrixCmd matrixCmd)
        {
            using (TcpClient client = new TcpClient())
            {
                IAsyncResult ar = client.BeginConnect(ip, 10001, null, null);
                WaitHandle wh = ar.AsyncWaitHandle;
                try
                {
                    if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5), false))
                    {
                        LogCtrl.ThreadSafeError("Matrix: Can't connect");
                        return;
                    }

                    Stream stm = client.GetStream();
                    byte[] bb = new byte[9];
                    stm.Write(matrixCmd.message, 0, matrixCmd.message.Length);
                    stm.Read(bb, 0, 9);

                    if (Encoding.UTF8.GetString(bb).Trim() == matrixCmd.response)
                        LogCtrl.ThreadSafeSuccess(matrixCmd.successText);
                    else LogCtrl.ThreadSafeError(matrixCmd.failText);

                    client.EndConnect(ar);
                }
                catch(Exception e)
                {
                    LogCtrl.ThreadSafeError("Matrix: Error connecting.");
                    LogCtrl.ThreadSafeError(e.Message);
                }
                finally
                {
                    wh.Close();
                }
            }
        }
    }
}
