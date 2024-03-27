using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

using Networking.Serialization;
using Networking.TCP;


class Program
{
    public static Dictionary<TcpClient, IrisPerson> users = new Dictionary<TcpClient, IrisPerson>();

    public static int curIrisProtocol = 312;

    static void Main(string[] args)
    {
        TCPServer server = new TCPServer();
        server.OnServerEvent += onEvent;

        server.OpenServer(3344);
        Console.ReadLine();
    }

    private static void onEvent(object sender, TCPArgs e)
    {
        INIConfig clientIni = new INIConfig(e.String);

        Console.WriteLine("IC: " + clientIni["Type"]);

        switch (clientIni["Type"])
        {
            case "HandshakePacket":
                try
                {
                    if (!users.ContainsKey(e.Client))
                    {
                        IrisPerson person = new IrisPerson();
                        person.displayName = clientIni["displayName"];
                        person.irisProtocol = int.Parse(clientIni["irisProtocol"]);

                        if (person.irisProtocol != curIrisProtocol)
                        {
                            DisconnectPacket pkt = new DisconnectPacket("Invalid iris protocol version");
                            pkt.SendTo(e.Client);
                            return;
                        }

                        users.Add(e.Client, person);

                        TextPacket txtPkt = new TextPacket("System", $"{person.displayName} has joined the chatroom");

                        txtPkt.SendTo(e.Client);
                        
                        // new user joined the chatroom
                        foreach (KeyValuePair<TcpClient, IrisPerson> user in users)
                        {
                            txtPkt.SendTo(user.Key);
                        }
                        break;
                    }
                    else
                    {
                        DisconnectPacket pkt = new DisconnectPacket("Invalid iris protocol version");
                        pkt.SendTo(e.Client);
                        return;
                    }
                }
                catch
                {
                    DisconnectPacket pkt = new DisconnectPacket("Internal server error");
                    pkt.SendTo(e.Client);
                    return;
                }
                break;
            case "TextPacket":
                break;
        }
    }

    public static void SendPacket(TcpClient client, INIConfig config)
    {
        Console.WriteLine("OG: " + config["Type"]);

        var sendBuffer = Encoding.UTF8.GetBytes(config.WriteConfigs());
        client.GetStream().Write(sendBuffer, 0, sendBuffer.Length);
    }
}