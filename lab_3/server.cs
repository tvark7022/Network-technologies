using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

using ZeroMQ;

namespace Examples
{
    static partial class Program
    {
        public static void Main(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.WriteLine("Привет, добро пожаловать на сервер.");
                Console.WriteLine();
            }
            
            using (var context = new ZContext())
            using (var responder = new ZSocket(context, ZSocketType.REP))
            {
                responder.Bind("tcp://*:5555");

                while (true)
                {
                    using (ZFrame request = responder.ReceiveFrame())
                    {
                        string receivedString = request.ReadString();
                        Console.WriteLine("Received {0}", receivedString);
                        
                        Thread.Sleep(1);
                        
                        string a = Encoding.UTF8.GetBytes(receivedString).Length.ToString();
                        responder.Send(new ZFrame(a));
                    }
                }
            }
        }
    }
}
