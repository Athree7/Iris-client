using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Networking.TCP
{
    public class TCPArgs
    {
        public TCPArgs(TcpClient Client, string String)
        {
            this.Client = Client;
            this.String = String;
        }

        public TcpClient Client { get; private set; }
        public string String { get; private set; }
    }

    public class TCPServer
    {
        public TcpListener tcpListener;
        public int ReceiveTimeout = 5000; // Set the receive timeout to 5 seconds
        public int SendTimeout = 5000; // Set the send timeout to 5 seconds
        public int MaxBufferSize = 10000;

        public EventHandler<TCPArgs> OnServerEvent;
        public EventHandler<string> OnServerError;

        private async Task Error(string error)
        {
            if (OnServerError != null)
            {
                OnServerError.Invoke(null, error);
            }
            else
            {
                throw new Exception(error);
            }
        }

        public async Task OpenServer(int port)
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Loopback, port);
                tcpListener.Start();

                while (true)
                {
                    var client = await tcpListener.AcceptTcpClientAsync();
                    client.ReceiveTimeout = ReceiveTimeout;
                    client.SendTimeout = SendTimeout;

                    _ = HandleClientAsync(client);
                }
            }
            catch (Exception ex)
            {
                await Error("Failed to setup connection: " + ex.Message);

                await OpenServer(port);
                return;
            }
        }

        public async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                var stream = client.GetStream();
                var buffer = new byte[MaxBufferSize];
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                if (OnServerEvent != null)
                    OnServerEvent.Invoke(null, new TCPArgs(client, message)); // send event to handle client event
            }
            catch (Exception ex)
            {
                await Error("Failed to asynchronously handle client request: " + ex.Message);

                await HandleClientAsync(client);
                return;
            }
        }
    }
}
