using Networking.Serialization;

class HandshakePacket : Packet
{
    public HandshakePacket(string displayName)
    {
        INIConfig sysMsg = new INIConfig("");
        sysMsg["Type"] = "HandshakePacket";
        sysMsg["displayName"] = displayName;
        sysMsg["irisProtocol"] = Program.curIrisProtocol.ToString();

        this.Value = sysMsg;
    }
}
