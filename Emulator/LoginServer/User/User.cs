using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

using LoginServer.Network;
using LoginServer.Network.Packets;

namespace LoginServer
{
    // SUGGESTION: The socket should be contained in it's own class to split networking and the user data.
    public class User
    {
        public const byte MIN_LENGTH = 7;
        public const byte HEADER_SIZE = 2;

        public User(Socket socket)
        {
            this.socket = socket;
            this.OnPacketReceived += User_OnPacketReceived;

            this.socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceive), null);
            this.Send(new WelcomePacket());            
        }

        public event EventHandler<PacketReceivedEventArgs> OnPacketReceived;
        private void User_OnPacketReceived(object sender, PacketReceivedEventArgs e)
        {
            foreach (InPacket packet in e.Packets)
            {
                try
                {
                    Handler h = PacketTable.Get(packet.OpCode);
                    if (h != null)
                    {
                        h.Handle((User)sender, packet);
                    }
                    else
                    {
                        Console.WriteLine("Received a packet with an unknown opCode: {0}", packet.OpCode);
                    }

                }
                catch (Exception ex) { Console.WriteLine("An error occured while processing a packet."); Console.WriteLine(ex.ToString()); } // Avoid disconnecting when handeling a packet.
            }
        }

        #region Networking

        private Socket socket;
        private byte[] receiveBuffer = new byte[0x800];
        private byte[] cachedBuffer = new byte[0];
        private bool isDisconnected = false;

        public void Send(byte[] buffer)
        {
            try { socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), null); }
            catch { Disconnect(); }
        }

        public void Send(OutPacket p)
        {
            try { Send(p.GetBytes()); }
            catch { Disconnect(); }
        }

        private void SendCallback(IAsyncResult iAr)
        {
            try { socket.EndSend(iAr); }
            catch { Disconnect(); }
        }

        private void OnDataReceive(IAsyncResult iAr)
        {
            try
            {
                int receivedLength = socket.EndReceive(iAr);

                // Disconnect / Connection loss
                if (receivedLength == 0) 
                {
                    Disconnect();
                    return;
                }

                // Make big buffer to store the cache and the new data.
                byte[] fullBuffer = new byte[cachedBuffer.Length + receivedLength];

                // Copy the oldest first if needed.
                if (cachedBuffer.Length > 0)
                    Array.Copy(cachedBuffer, 0, fullBuffer, 0, cachedBuffer.Length);

                // Add the fresh bytes
                Array.Copy(receiveBuffer, 0, cachedBuffer, cachedBuffer.Length, receivedLength);

                // Check if we can process the data.
                bool keepProcessing = fullBuffer.Length >= MIN_LENGTH;

                // Remember current offset.
                int offset = 0;

                // Variables for decryption.
                byte key = 0;
                byte length = 0;
                byte[] packetContent = new byte[0];

                // Parsed packets.
                List<InPacket> receivedPackets = new List<InPacket>();

                while (keepProcessing)
                {
                    // Decryption foreach packet invidiualy.
                    key = (byte)(fullBuffer[offset] ^ 0xDE);
                    length = (byte)(fullBuffer[offset + 1] ^ key);

                    // Check the length.
                    if (fullBuffer.Length - offset >= length)
                    {
                        // The remaining buffer is long enough.
                        packetContent = new byte[length - HEADER_SIZE];

                        // Parse the content.
                        for (byte i = 0; i < length; i++)
                        {
                            packetContent[i] = (byte)(fullBuffer[offset + HEADER_SIZE + i] ^ key);
                        }

                        // Push to the received list
                        receivedPackets.Add(new InPacket(packetContent));

                        // Increase the offset
                        offset += length;

                    }
                    else
                    {
                        break; // Exit the while loop.
                    }

                    keepProcessing = (fullBuffer.Length - offset) >= MIN_LENGTH;
                }

                if (offset < fullBuffer.Length)
                {
                    // We have got some extra data, we should cache it.
                    cachedBuffer = new byte[fullBuffer.Length - offset];
                    // Copy the remaining bytes to the cache.

                }

                // Invoke handeling if there is a complete packet.
                if (receivedPackets.Count > 0 && OnPacketReceived != null)
                {
                    User_OnPacketReceived(this, new PacketReceivedEventArgs(receivedPackets.ToArray()));
                }

                // Receive more data.
                socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceive), null);
            }
            catch
            {
                Disconnect();
            }
        }

        internal void Disconnect()
        {
            if (isDisconnected) return;
            isDisconnected = true;

            try { socket.Close(); }
            catch { /* Do nothing */ }

            Console.WriteLine("Client Disconnected!");

            // Call Disconnect stuff here.

        }

        #endregion
    }
}
