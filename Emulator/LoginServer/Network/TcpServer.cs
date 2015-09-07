using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Shared;

namespace LoginServer.Network
{
    public class TcpServer
    {
        private Socket socket;
        public TcpServer(ushort port)
        {
            try
            {
                this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.socket.Bind(new IPEndPoint(IPAddress.Any, port));
                this.socket.Listen(0);
                this.socket.BeginAccept(new AsyncCallback(this.Accept), null);
            }
            catch { }

            if (this.HasInit)
            {
                Log.WriteLine("Tcp Socket has been bound to port " + port);
            }
        }

        public bool HasInit { get { return this.socket != null && this.socket.IsBound; } } 

        private void Accept(IAsyncResult iAr)
        {
            Socket s = null;
            try
            {
                s = socket.EndAccept(iAr);
                Log.WriteLine("New connection from " + s.RemoteEndPoint.ToString());
                User usr = new User(s);                
            }
            catch { try { s.Close(); } catch { } } // Force close the socket when an exception is triggered here to avoid ghost sockets.

            if (socket != null)
            {
                socket.BeginAccept(new AsyncCallback(this.Accept), null);
            }
        }
    }
}
