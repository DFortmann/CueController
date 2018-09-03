using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace CueController3.Controller.Network
{
    class MyUdpClient
    {
        private const int UDPPORT = 4568;

        public void Send(string ip, string message)
        {
            Thread thread = new Thread(() => SendMethod(ip, message));
            thread.Start();
        }

        private void SendMethod(string ip, string message)
        {
            using (UdpClient udpClient = new UdpClient(ip, UDPPORT))
            {
                try
                {
                    Byte[] inputToBeSent = new Byte[256];
                    inputToBeSent = Encoding.ASCII.GetBytes(message.ToCharArray());
                    IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Parse(ip), UDPPORT);
                    udpClient.Send(inputToBeSent, inputToBeSent.Length);
                    udpClient.Close();
                }
                catch
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        LogCtrl.Error("Error connection to Backup CueController!");
                    }));
                }
            }
        }
    }
}