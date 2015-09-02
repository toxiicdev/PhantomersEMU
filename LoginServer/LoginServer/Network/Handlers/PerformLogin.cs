using System;

namespace LoginServer.Network.Handlers
{
    public class PerformLogin : Handler
    {
        public void Handle(User u, InPacket p)
        {
            ushort usernameLength = p.ReadUShort();
            string username = p.ReadString(usernameLength);
            ushort passwordLength = p.ReadUShort();
            string password = p.ReadString(passwordLength);

            Console.WriteLine("Login Request: {0}:{1}", username, password);
        }
    }
}
