using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace buChajt
{
    class Program
    {
        private static TcpClient user;
        private static NetworkStream incomingNetworkStream;
        private static NetworkStream outcomingNetworkStream;

        static void Main(string[] args)
        {
            Console.WriteLine("What's your nickname?");
            string usrName;
            usrName = Console.ReadLine();

            



            user = new TcpClient();
            user.Connect("127.0.0.1", 8080);
            Console.WriteLine("Connected successfully.");
            incomingNetworkStream = user.GetStream();
            byte[] outBuffer = System.Text.Encoding.ASCII.GetBytes(String.Concat(usrName, "$"));
            incomingNetworkStream.Write(outBuffer, 0, outBuffer.Length);
            incomingNetworkStream.Flush();

            Thread incMsgThread = new Thread(handleIncomingMessages);
            incMsgThread.Start();
            

            while (true)
            {
                Console.ReadKey(true);
                outcomingNetworkStream = user.GetStream();
                Console.WriteLine("YOUR MESSAGE:");
                //Console.SetCursorPosition(0, Console.CursorTop - 1);
                string msgToSend = Console.ReadLine();
                byte[] sendBuffer = System.Text.Encoding.ASCII.GetBytes(String.Concat(msgToSend, "$"));
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                ConsoleUtils.ClearConsoleLine();
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                ConsoleUtils.ClearConsoleLine();
                outcomingNetworkStream.Write(sendBuffer, 0, sendBuffer.Length);
                outcomingNetworkStream.Flush();
            }

        }

        private static void handleIncomingMessages()
        {
            while (true)
            {
                incomingNetworkStream = user.GetStream();
                byte[] inBuffer = new byte[67000];
                incomingNetworkStream.Read(inBuffer, 0, user.ReceiveBufferSize);
                incomingNetworkStream.FlushAsync();
                string toBeWritten = System.Text.Encoding.ASCII.GetString(inBuffer);
                toBeWritten = toBeWritten.Substring(0, toBeWritten.LastIndexOf('$'));
                Console.WriteLine(toBeWritten);
                //Console.SetCursorPosition(0, Console.CursorTop - 1);

            }
        }

       
    }
}
