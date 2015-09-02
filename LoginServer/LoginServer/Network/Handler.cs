namespace LoginServer.Network
{
    /// <summary>
    /// An interface that handles packet handeling.
    /// </summary>
    public interface Handler
    {
        void Handle(User u, InPacket p);
    }
}
