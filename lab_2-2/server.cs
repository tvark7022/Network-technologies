using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            IPHostEntry ipHost = Dns.GetHostEntry("192.168.1.37");
            IPAddress ipAddr = Array.FindAll(ipHost.AddressList, a => a.AddressFamily == AddressFamily.InterNetwork)[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 5354);
            
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);
                
                while (true)
                {
                    Console.WriteLine("We are waiting for the connection through the port {0}", ipEndPoint + "\n");
                    
                    Socket handler = sListener.Accept();
                    string data = null;
                    
                    byte[] bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);

                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    
                    Console.WriteLine(IPAddress.Parse(((IPEndPoint)handler.RemoteEndPoint).Address.ToString()) + ":");
                    Console.Write("Received text: " + data + "\n\n");
                    
                    string reply = "Long Message " + data.Length.ToString()
                            + " Characters";
                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    handler.Send(msg);

                    if (data.IndexOf("exit") > -1)
                    {
                        Console.WriteLine("Server OFF.");
                        break;
                    }
                    
                    handler.Close();
                }
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
    }
}

