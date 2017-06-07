using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Work2Client
{
    class Program {
        static void Main(string[] args) {
            IPAddress serverAddress = IPAddress.Parse("192.168.1.37");
            int port = 1255;
            
            DateTime time = DateTime.Now;
            Console.WriteLine("Моё время " + time.ToString("dd.MM.yyyy   HH:mm:ss"));
            
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint serverEndPoint = new IPEndPoint(serverAddress, port);
            
            byte[] bytes = BitConverter.GetBytes(time.ToBinary());
            socket.SendTo(bytes, serverEndPoint);
            
            byte[] buffer = new byte[sizeof(long)];
            socket.Receive(buffer);
			socket.Close();
            
            Console.WriteLine("Разница во времени с сервером: " + BitConverter.ToInt32(buffer, 0) + "ms");

            Console.ReadKey();
        }
    }
}
