using System;
using System.Collections.Generic;
using System.Text;

namespace LoginServer.Packets_Structure
{
    class PacketTable
    {
        public static Dictionary<int, Handler> packetTable = new Dictionary<int, Handler>();

        public static void Load()
        {
            packetTable = new Dictionary<int, Handler>
            {
                { 0x4E210000, new Packets.Handlers.PerformLogin() }
            };
        }

        public static Handler FindHandler(byte[] packetData)
        {
            int packetId = BitConverter.ToInt32(packetData, 2);
            if(packetTable.ContainsKey(packetId))
            {
                return packetTable[packetId];
            }
            return null;
        }
    }
}
