using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ZeroMQ;

namespace Examples
{
    static partial class Program
    {
        public static void Main(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.WriteLine("    Клиент подключен к адресу");
                Console.WriteLine("               tcp://192.168.1.37:5555");
                Console.WriteLine();
                args = new string[] { "tcp://192.168.1.37:5555" };
            }

            string endpoint = args[0];
            
            using (var context = new ZContext())
            using (var requester = new ZSocket(context, ZSocketType.REQ))
            {
                requester.Connect(endpoint);
                
                Console.WriteLine("Отправь сообщение: \n");
                string requestText = Console.ReadLine();
                Console.WriteLine("Sending {0}…", requestText);
                Console.WriteLine();
                
                requester.Send(new ZFrame(requestText));
                
                using (ZFrame reply = requester.ReceiveFrame())
                {
                    Console.WriteLine(" Вы отправили сообщение: {0}, Длина сообщения: {1}\n", requestText, reply.ReadString());
                }
            }
            Console.ReadKey();
        }
    }
}

