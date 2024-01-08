using Oxide.Core.Libraries;
using Oxide.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Oxide.Ext.WebSocket.Libraries
{
    public class WebSocketSecure : Library, IWebSocket
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
            X509Certificate2 certificate = new X509Certificate2("C:\\Users\\...\\Desktop\\cert\\certificate.pfx", "passwordtofile");
            server.Start();

            while (true)
            {
                TcpClient sock = server.AcceptTcpClient();

                SslStream sslStream = new SslStream(sock.GetStream(), true);

                try
                {
                    sslStream.AuthenticateAsServer(certificate, false, SslProtocols.Tls13, true);
                    Console.WriteLine("Client connected. Authenticating...");

                    string incomingMessage = "";


                    byte[] buffer = new byte[2048];
                    int bytes = -1;
                    do
                    {
                        bytes = sslStream.Read(buffer, 0, buffer.Length);

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
                    sslStream.Write(outbuffer);
                    sslStream.Flush();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    sslStream.Close();
                    sock.Close();
                }
            }
        }

        void IWebSocket.Shutdown() => WorkerThread.Abort();
    }
}
