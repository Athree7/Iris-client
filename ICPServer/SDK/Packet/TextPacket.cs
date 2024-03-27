using System;
using System.Collections.Generic;
using System.Net.Sockets;

using Networking.Serialization;

class TextPacket : Packet
{
    public TextPacket(string displayName, string value)
    {
        INIConfig sysMsg = new INIConfig("");
        sysMsg["Type"] = "TextPacket";
        sysMsg["irisProtocol"] = Program.curIrisProtocol.ToString();
        sysMsg["displayName"] = displayName;
        sysMsg["value"] = value;

        this.Value = sysMsg;
    }
}
