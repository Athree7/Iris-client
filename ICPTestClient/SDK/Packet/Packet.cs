using Networking.Serialization;
using System.Net.Sockets;
using System.Text;

class Packet
{
    public INIConfig Value { get; set; }

    public virtual void Send()
    {
        Program.SendPacket(this.Value);
    }
}