using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Oxide.Core;
using Oxide.Core.Libraries;
using Oxide.Core.Plugins;

namespace Oxide.Ext.WebSocket.Libraries
{
    public class WebSocket : Library, IWebSocket
    {
        private Thread WorkerThread = null;

        void IWebSocket.Initialize()
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
            TcpListener server = new TcpListener(IPAddress.Any, 1111);
            server.Start();

            while (true)
            {
                TcpClient sock = server.AcceptTcpClient();
                NetworkStream stream = sock.GetStream();

                string incomingMessage = "";

                byte[] buffer = new byte[2048];
                int bytes = -1;
                do
                {
                    bytes = stream.Read(buffer, 0, buffer.Length);

                    incomingMessage += Encoding.ASCII.GetString(buffer, 0, bytes);
                    if (incomingMessage.IndexOf("<EOF>") != -1 || incomingMessage.Contains("\r\n\r\n"))
                    {
                        break;
                    }
                } while (bytes != 0);

                if (incomingMessage != "")
                {
                    Interface.Oxide.LogInfo(incomingMessage);
                    Interface.CallHook("OnWebSocketMessage", incomingMessage);
                }

                string response = "HTTP/1.1 200 OK\nServer: Apache / 2.2.14(Win32)\nContent - Length: 16\nContent - Type: text / html\nConnection: Closed\nContent-Type: text/html\n\n<html>Ok</html>";
                byte[] outbuffer = Encoding.ASCII.GetBytes(response.ToString());
                stream.Write(outbuffer, 0, outbuffer.Length);
                stream.Flush();
                sock.Close();
            }



        }

        void IWebSocket.Shutdown() => WorkerThread.Abort();
    }
}
