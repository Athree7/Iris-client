using System.Collections.Generic;
using System.Net.Sockets;

using Networking.Serialization;

class DisconnectPacket : Packet
{
    public DisconnectPacket(string reason)
    {
        INIConfig DisconnectedPacket = new INIConfig("");
        DisconnectedPacket["Type"] = "DisconnectPacket";
        DisconnectedPacket["Value"] = reason;
        DisconnectedPacket["irisProtocol"] = Program.curIrisProtocol.ToString();

        this.Value = DisconnectedPacket;
    }

    public override void SendTo(TcpClient client)
    {
        if (Program.users.ContainsKey(client))
        {
            Program.users.Remove(client);
        }

        base.SendTo(client);
    }
}
