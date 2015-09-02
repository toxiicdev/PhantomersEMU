using System;
namespace LoginServer.Network
{
    public class PacketReceivedEventArgs : EventArgs
    {
        public readonly InPacket[] Packets;
        public PacketReceivedEventArgs(InPacket[] packets) : base()
        {
            this.Packets = packets;
        }
    }
}
