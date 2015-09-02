using LoginServer.Enums;

namespace LoginServer.Network.Packets
{
    public class WelcomePacket : OutPacket
    {
        public WelcomePacket() : base(PacketType.WELCOME)
        {
            WriteHexString("E71A2512510BFC09");
        }
    }
}
