using System;

using Networking.Serialization;
using Networking.TCP;

class Program
{
    public static TCPClient client = new TCPClient("127.0.0.1", 3344);
    public static int curIrisProtocol = 312;

    public static void Main(string[] args)
    {
        client.OnClientEvent += OnClientEvent;

        HandshakePacket hsPkt = new HandshakePacket("Username1");
        hsPkt.Send();
        Console.ReadLine();
    }

    public static void OnClientEvent(object sender, TCPArgs e)
    {
        INIConfig pkt = new INIConfig(e.String);

        Console.WriteLine("IC: " + pkt["Type"]);
    }

    public static void SendPacket(INIConfig config)
    {
        Console.WriteLine("OG: " + config["Type"]);
        client.SendData(config.WriteConfigs());
    }
}