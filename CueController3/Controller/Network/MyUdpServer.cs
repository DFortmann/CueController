using CueController3.Controller.Cues;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace CueController3.Controller.Network
{
    class MyUdpServer
    {
        private const int UDPPORT = 4568;
        private Thread udpThread;
        private volatile bool _shouldStop;
        private IPEndPoint ipep = new IPEndPoint(IPAddress.Any, UDPPORT);
        private IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        private UdpClient client;
        byte[] data = new byte[1024];

        public bool Init()
        {
            try
            {
                client = new UdpClient(ipep);
                client.Client.ReceiveTimeout = 5000;
            }
            catch (Exception e)
            {
                LogCtrl.Error("Couldn't create Backup Listener thread!");
                LogCtrl.Error(e.Message);
                return false;
            }
            return true;
        }

        public void Start()
        {
            try
            {
                udpThread = new Thread(StartReceive);
                udpThread.Start();
            }
            catch (Exception e)
            {
                LogCtrl.Error("Couldn't start Go Listener thread!");
                LogCtrl.Error(e.Message);
                udpThread.Abort();
            }
        }

        public void RequestStop()
        {
            _shouldStop = true;
        }

        private void StartReceive()
        {
            try
            {
                while (!_shouldStop)
                {
                    data = client.Receive(ref sender);
                    string[] message = Encoding.ASCII.GetString(data, 0, data.Length).Split(' ');
                    if (message.Length == 2)
                    {
                        int cueNr;
                        if (int.TryParse(message[1], out cueNr))
                        {
                            switch (message[0])
                            {
                                case "GO":
                                    Application.Current.Dispatcher.Invoke(new Action(() =>
                                    {
                                        GoCtrl.Go(cueNr);
                                    }));
                                    break;
                                case "BACK":
                                    Application.Current.Dispatcher.Invoke(new Action(() =>
                                    {
                                        GoCtrl.GoBack();
                                    }));
                                    break;
                            }
                        }

                    }
                }
            }
            catch (SocketException se)
            {
                if (se.SocketErrorCode == SocketError.TimedOut) Start();
                else
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        LogCtrl.Error("Backup listener thread crashed!");
                        LogCtrl.Error(se.Message);
                    }));
            }
        }
    }
}