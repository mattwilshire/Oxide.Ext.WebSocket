using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Oxide.Core;
using Oxide.Core.Libraries;

namespace Oxide.Ext.WebSocket.Libraries
{
    internal class WebSocket : Library
    {
        private Thread WorkerThread = null;

        internal void Initialize()
        {
            if ((WorkerThread == null) || (!WorkerThread.IsAlive))
            {
                WorkerThread = new Thread(Worker);
                WorkerThread.IsBackground = true;
                WorkerThread.Start();
            }

            Interface.Oxide.LogWarning("Extension Initialized!");
        }

        private void Worker()
        {
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 1111);
            server.Start();

            while (true)
            {
                Console.WriteLine(1);
                Socket sock = server.AcceptSocket();
                Console.WriteLine(2);
                byte[] buffer = new byte[1024];

                string incomingMessage = "";

                if (sock.Available < 1)
                {
                    sock.Close();
                    continue;
                }

                while (sock.Available > 0)
                {
                    Console.WriteLine(3);
                    int gotBytes = sock.Receive(buffer);
                    incomingMessage += Encoding.ASCII.GetString(buffer, 0, gotBytes);
                }

                if (incomingMessage != "")
                {
                    Interface.Oxide.LogInfo(incomingMessage);
                    Interface.CallHook("OnWebSocketMessage", incomingMessage);
                }

                string response = "HTTP/1.1 200 OK\nServer: Apache / 2.2.14(Win32)\nContent - Length: 16\nContent - Type: text / html\nConnection: Closed\nContent-Type: text/html\n\n<html>Ok</html>";
                byte[] outbuffer = Encoding.ASCII.GetBytes(response.ToString());
                sock.Send(outbuffer);
                sock.Close();
            }


           
        }

        internal void Shutdown() => WorkerThread.Abort();
    }
}
