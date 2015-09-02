using System;
using System.Text;
using System.IO;

using LoginServer.Enums;

namespace LoginServer.Network
{
    public class OutPacket : Packet
    {
        private BinaryWriter writer;

        public OutPacket(PacketType opCode)
            : base()
        {
            OpCode = opCode;
            writer = new BinaryWriter(this.memoryStream);

            // Reserve the header + add the opCode
            writer.Write((ushort)0); // Reserve Key + Length.
            writer.Write((int)opCode);
        }
        

        public void WriteHexString(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

            this.writer.Write(bytes);
        }

        public void Write(bool value)
        {
            this.writer.Write((byte)(value ? 1 : 0));
        }

        public void Write(byte value)
        {
            this.writer.Write(value);
        }

        public void Write(byte[] value)
        {
            this.writer.Write(value);
        }

        public void Write(short value)
        {
            this.writer.Write(value);
        }

        public void Write(ushort value)
        {
            this.writer.Write(value);
        }

        public void Write(int value)
        {
            this.writer.Write(value);
        }

        public void Write(uint value)
        {
            this.writer.Write(value);
        }

        public void Write(long value)
        {
            this.writer.Write(value);
        }

        public void Write(ulong value)
        {
            this.writer.Write(value);
        }

        private static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public void Write(string value)
        {
            if (value.Length > byte.MaxValue)
                throw new IndexOutOfRangeException();

            byte[] result = Encoding.GetEncoding(1251).GetBytes(value);
            Write((byte)result.Length);
            Write(result);
        }

        public byte[] GetBytes()
        {
            Write((byte)0xED);

            byte dynamicKey = (byte)(new Random().Next(1, 256));

            byte[] buffer = this.memoryStream.ToArray();

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
