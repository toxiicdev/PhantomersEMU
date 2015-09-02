using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace LoginServer
{
    class Handler
    {
        BinaryReader reader;
        MemoryStream ms;

        public int packetId;

        private byte[] buffer;

        private byte dynamicKey, length;

        public void SetData(byte[] buffer)
        {
            Array.Resize(ref buffer, buffer.Length - 3);
            this.buffer = buffer;

            this.ms = new MemoryStream(this.buffer);
            this.reader = new BinaryReader(ms);
            
            this.packetId = reader.ReadInt32();
        }

        public virtual void Handle()
        {
            /* Handler gets overwritten */
        }
    }
}
