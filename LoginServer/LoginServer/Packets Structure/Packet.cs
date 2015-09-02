using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace LoginServer
{
    class Packet
    {
        BinaryWriter writer;
        MemoryStream ms;
        public void SetID(int id)
        {
            this.ms = new MemoryStream(0);
            this.writer = new BinaryWriter(this.ms);

            this.writer.Write((ushort)0);
            this.writer.Write((int)id);
        }

        public void Write(byte value)
        {
            this.writer.Write(value);
        }

        public void WriteHexString(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

            this.writer.Write(bytes);
        }

        public byte[] GetBytes()
        {
            this.Write((byte)0xED);

            byte dynamicKey = (byte)(new Random().Next(1, 256));

            byte[] buffer = this.ms.ToArray();

            buffer[0] = (byte)(dynamicKey ^ 0xDE);
            buffer[1] = (byte)buffer.Length;

            for (int i = 1; i < buffer.Length; i++)
            {
                buffer[i] ^= dynamicKey;
            }

            return buffer;
        }
    }
}
