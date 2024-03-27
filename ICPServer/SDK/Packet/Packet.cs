using Networking.Serialization;
using System.Net.Sockets;

class Packet
{
    public INIConfig Value { get; set; }

    public virtual void SendTo(TcpClient client)
    {
        Program.SendPacket(client, this.Value);
    }
}