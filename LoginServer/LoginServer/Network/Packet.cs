using System.IO;
using LoginServer.Enums;

namespace LoginServer.Network
{
    public abstract class Packet
    {
        public PacketType OpCode { get; set; }

        protected MemoryStream memoryStream = null;

        public Packet()
        {
            this.memoryStream = new MemoryStream();
        }
        
    }
}
