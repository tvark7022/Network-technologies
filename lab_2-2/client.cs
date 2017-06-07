using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SendMessageFromSocket(5354);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }

        static void SendMessageFromSocket(int port)
        {
            byte[] bytes = new byte[1024];
            
            IPHostEntry ipHost = Dns.GetHostEntry("192.168.1.37");
            IPAddress ipAddr = Array.FindAll(ipHost.AddressList, a => a.AddressFamily == AddressFamily.InterNetwork)[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            
            sender.Connect(ipEndPoint);

            Console.Write("Send Message: ");
            string message = Console.ReadLine();

            Console.WriteLine("\nSocet connect with {0} ", sender.RemoteEndPoint.ToString());
            byte[] msg = Encoding.UTF8.GetBytes(message);
            
            int bytesSent = sender.Send(msg);
            
            int bytesRec = sender.Receive(bytes);

            Console.WriteLine("\nServer answer: {0}\n", Encoding.UTF8.GetString(bytes, 0, bytesRec));
            
            if (message.IndexOf("exit") == -1)
                SendMessageFromSocket(port);
            
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
    }
}
