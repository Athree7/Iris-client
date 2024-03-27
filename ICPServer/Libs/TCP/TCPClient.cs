using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Networking.TCP
{
    public class TCPClient
    {
        public string IPAddress { get; private set; }
        public int Port { get; private set; }
        public TCPClient(string IPAddress, int Port)
        {
            this.IPAddress = IPAddress;
            this.Port = Port;
        }

        public int MaxBufferSize = 10000;

        public EventHandler<TCPArgs> OnClientEvent;
        public EventHandler<string> OnClientError;

        private async Task Error(string error)
        {
            if (OnClientError != null)
            {
                OnClientError.Invoke(null, error);
            }
            else
            {
                throw new Exception(error);
            }
        }

        public async Task<string> SendData(string data)
        {
            try
            {
                TcpClient Client = new TcpClient();
                await Client.ConnectAsync(IPAddress, Port);

                NetworkStream stream = Client.GetStream();
                byte[] sendBuffer = Encoding.UTF8.GetBytes(data);
                await stream.WriteAsync(sendBuffer, 0, sendBuffer.Length);

                byte[] buffer = new byte[MaxBufferSize];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Client.Close();

                return receivedMessage; // return response
            }
            catch (Exception ex)
            {
                await Error("Failed to send data: " + ex.Message);

                return await SendData(data);
            }
        }
    }
}
