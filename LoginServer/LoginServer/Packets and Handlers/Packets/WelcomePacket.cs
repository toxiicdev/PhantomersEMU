using System;
using System.Collections.Generic;
using System.Text;

namespace LoginServer.Packets_and_Handlers.Packets
{
    class WelcomePacket : Packet
    {
        public WelcomePacket()
        {
            this.SetID(0x27110000);
            this.WriteHexString("E71A2512510BFC09");
        }
    }
}
