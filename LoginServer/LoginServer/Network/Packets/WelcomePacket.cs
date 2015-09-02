using LoginServer.Enums;

namespace LoginServer.Network.Packets
{
    public class WelcomePacket : OutPacket
    {
        public WelcomePacket() : base(PacketType.DO_A_LOGIN)
        {
            WriteHexString("E71A2512510BFC09");
        }
    }
}
