using System;
using System.Collections.Generic;

using LoginServer.Enums;
using LoginServer.Network.Handlers;

namespace LoginServer.Network
{
    public static class PacketTable
    {
        public static Dictionary<PacketType, Handler> packetTable = new Dictionary<PacketType, Handler>();

        public static void Load()
        {
            packetTable = new Dictionary<PacketType, Handler>
            {
                { PacketType.DO_A_LOGIN, new PerformLogin()}
            };
        }

        public static Handler Get(PacketType opCode)
        {
            if (packetTable.ContainsKey(opCode))
                return packetTable[opCode];
            return null;
        }
    }
}
