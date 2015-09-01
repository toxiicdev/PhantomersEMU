using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace LoginServer
{
    class User
    {
        public User(Socket socket)
        {
            this.socket = socket;

            this.Send(new Packets_and_Handlers.Packets.WelcomePacket());
        }
        
        #region Networking
        Socket socket;
        byte[] buffer = new byte[1024];

        public void Send(Packet p)
        {
            try { byte[] sendBuffer = p.GetBytes(); if (sendBuffer != null) { socket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(sendCallBack), null); } }
            catch { this.Kick(); }
        }

        public void SendBuffer(byte[] buffer)
        {
            try { if (buffer != null) { socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(sendCallBack), null); } }
            catch { this.Kick(); }
        }

        private void sendCallBack(IAsyncResult iAr)
        {
            try { socket.EndSend(iAr); }
            catch { this.Kick(); }
        }

        public void OnDataReceive(IAsyncResult callback)
        {
            int dataLength = socket.EndReceive(callback);

            if(dataLength > 0)
            {
                try
                {
                    byte[] packetData = new byte[dataLength];
                    Array.Copy(buffer, packetData, dataLength);
                    
                    /* Decrpyting packet */

                    string data = "";

                    for (int i = 1; i < packetData.Length; i++)
                    {
                        packetData[i] ^= (byte)(packetData[0] ^ 0xDE);
                        data += packetData[i];
                    }

                    Log.WriteDebug(data);


                    Handler h = Packets_Structure.PacketTable.FindHandler(packetData);

                    if(h != null)
                    {
                        h.SetData(packetData);
                        h.Handle();
                    }
                    else
                    {
                        /* DEBUG DATA */

                        Log.WriteError("Unhandled packet: ");
                    }
                }
                catch
                {
                    /* Avoiding disconnections and other shit such as packet handling error */
                }
            }
            else
            {
                this.Kick();
            }
        }

        void Kick()
        {
            this.socket.Close();
        }

        #endregion
    }
}
