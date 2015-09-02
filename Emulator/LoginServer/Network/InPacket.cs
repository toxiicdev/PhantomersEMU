using System;
using System.IO;

using LoginServer.Enums;

namespace LoginServer.Network
{
    public class InPacket : Packet
    {
        public int Remaining { get { return (int)(memoryStream.Length - memoryStream.Position); } }

        private BinaryReader reader;

        /// <summary>
        /// Handles the decrypted content of the packet
        /// </summary>
        /// <param name="buffer">Decrypted packet content with the header removed</param>
        public InPacket(byte[] buffer)
        {
            // Parse the buffer, the header should be already removed and the content should already be decrypted.
            memoryStream = new MemoryStream(buffer);
            reader = new BinaryReader(memoryStream);

            // Parsing the OpCode
            OpCode = (PacketType)reader.ReadInt32();
        }

        public byte ReadByte()
        {
            if (Remaining < 1)
                throw new IndexOutOfRangeException();
            return reader.ReadByte();
        }

        public sbyte ReadSByte()
        {
            if (Remaining < 1)
                throw new IndexOutOfRangeException();
            return reader.ReadSByte();
        }

        public ushort ReadUShort()
        {
            if (Remaining < 2)
                throw new IndexOutOfRangeException();
            return reader.ReadUInt16();
        }

        public short ReadShort()
        {
            if (Remaining < 2)
                throw new IndexOutOfRangeException();
            return reader.ReadInt16();
        }

        public uint ReadUInt()
        {
            if (Remaining < 4)
                throw new IndexOutOfRangeException();
            return reader.ReadUInt32();
        }

        public int ReadInt()
        {
            if (Remaining < 4)
                throw new IndexOutOfRangeException();
            return reader.ReadInt32();
        }

        public ulong ReadULong()
        {
            if (Remaining < 8)
                throw new IndexOutOfRangeException();
            return reader.ReadUInt64();
        }

        public long ReadLong()
        {
            if (Remaining < 8)
                throw new IndexOutOfRangeException();
            return reader.ReadInt64();
        }

        public void Skip(int size)
        {
            if (Remaining < size)
                throw new IndexOutOfRangeException();
            reader.ReadBytes(size); // Skip
        }

        public string ReadString()
        {
            throw new NotImplementedException();
        }

        public string ReadString(int l)
        {
            if (Remaining < l)
                throw new IndexOutOfRangeException();

            byte[] stringBytes = ReadBytes(l);
            string str = System.Text.Encoding.GetEncoding(1251).GetString(stringBytes);

            int length = str.IndexOf(char.MinValue);
            if (length != -1)
                str = str.Substring(0, length);

            return str;
        }

        public byte[] ReadBytes(int len)
        {
            byte[] result = new byte[len];
            if (Remaining < result.Length)
                throw new IndexOutOfRangeException();
            this.reader.Read(result, 0, result.Length);
            return result;
        }
    }
}
