﻿using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CueController3.Controller.Beamer
{
    class BeamerCtrl
    {
        private byte[] closeCmd, openCmd;
        public string ip;
        public int id;

        public BeamerCtrl(int id, string ip)
        {
            this.id = id;
            this.ip = ip;
            ASCIIEncoding asen = new ASCIIEncoding();
            openCmd = asen.GetBytes("%1AVMT 30\r");
            closeCmd = asen.GetBytes("%1AVMT 31\r");
        }

        public void Shutter(bool open)
        {
            if (open)
            {
                LogCtrl.Status(string.Concat("Beamer ", id, ": Open (", ip, ")"));
                new Thread(() => ShutterMethod(true)).Start();
            }
            else
            {
                LogCtrl.Status(string.Concat("Beamer ", id, ": Close (", ip, ")"));
                new Thread(() => ShutterMethod(false)).Start();
            }
        }

        private void ShutterMethod(bool open)
        {
            using (TcpClient client = new TcpClient())
            {
                IAsyncResult ar = client.BeginConnect(ip, 4352, null, null);
                WaitHandle wh = ar.AsyncWaitHandle;
                try
                {
                    if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5), false))
                    {
                        LogCtrl.ThreadSafeError(string.Concat("Beamer ", id, ": Timed out. (", ip, ")"));
                        return;
                    }

                    byte[] bb = new byte[9];
                    Stream stm = client.GetStream();
                    stm.Read(bb, 0, 9);
                    if (open) stm.Write(openCmd, 0, openCmd.Length);
                    else stm.Write(closeCmd, 0, closeCmd.Length);
                    stm.Read(bb, 0, 9);

                    if (Encoding.UTF8.GetString(bb) == "%1AVMT=OK")
                    {
                        if (open) LogCtrl.ThreadSafeSuccess(string.Concat("Beamer ", id, ": Opened (", ip, ")"));
                        else LogCtrl.ThreadSafeSuccess(string.Concat("Beamer ", id, ": Closed (", ip, ")"));
                    }
                    else
                    {
                        if (open) LogCtrl.ThreadSafeError(string.Concat("Beamer ", id, ": Error opening. (", ip, ")"));
                        else LogCtrl.ThreadSafeError(string.Concat("Beamer ", id, ": Error closing. (", ip, ")"));
                    }

                    client.EndConnect(ar);
                }
                catch(Exception e)
                {
                    LogCtrl.ThreadSafeError(string.Concat("Beamer ", id, ": Error connecting. (", ip, ")"));
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
