using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace LoginServer
{
    class TcpServer
    {
        private Socket socket;
        public TcpServer(ushort port)
        {
            try
            {
                this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.socket.Bind(new IPEndPoint(IPAddress.Any, port));
                this.socket.Listen(0);
                this.socket.BeginAccept(this.Accept, socket);
            }
            catch { }

            if (this.HasInit)
            {
                Log.WriteLine("Tcp Socket has been bound to port " + port);
            }
        }

        public bool HasInit { get { return this.socket.IsBound; } }

        private void Accept(IAsyncResult callback)
        {
            Socket remoteSocket = socket.EndAccept(callback);

            if(remoteSocket.IsBound)
            {
                User usr = new User(remoteSocket);
                
                Log.WriteLine("New connection from " + remoteSocket.RemoteEndPoint.ToString());
            }
        }
    }
}
