using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace buChajt_Server
{
    class Program
    {
        public static Hashtable userList = new Hashtable();

        static void Main(string[] args)
        {
            TcpListener srvSocket;
            TcpClient userSocket;


            try
            {
                Console.WriteLine("Starting TCP Listener");
                srvSocket = new TcpListener(ServerConstants.serverAddress, ServerConstants.serverPort);
                srvSocket.Start();
                Console.WriteLine(String.Format("Server started successfully. Server IP:{0}, Port:{1}", ServerConstants.serverAddress, ServerConstants.serverPort));
                Byte[] byteBuffer = new Byte[100000];
                Console.WriteLine("Buffer created successfully");
                string userData;



                while (true)
                {
                    userSocket = srvSocket.AcceptTcpClient();
                    Console.WriteLine("Accepted TCP Client. Proceeding to NetworkStream.");

                    NetworkStream networkStream = userSocket.GetStream();
                    Console.WriteLine("Proceeding to read the NetworkStream");
                    networkStream.Read(byteBuffer, 0, userSocket.ReceiveBufferSize);
                    Console.WriteLine("Reading ended with success. Preparing to read data from User");
                    userData = System.Text.Encoding.ASCII.GetString(byteBuffer);
                    userData = userData.Substring(0, userData.IndexOf("$"));

                    userList.Add(userData, userSocket);
                    broadcastMessage(userData + " joined", userData, false);
                    Console.WriteLine(String.Format("{0} JOINED THE ROOM", userData));
                    UserHandler userHandler = new UserHandler(userSocket, userData, userList);

                }


                
                srvSocket.Stop();

            }
            catch (SocketException se)
            {
                Console.WriteLine(se.StackTrace);
            }
            



        }

        public static void broadcastMessage(string message, string userName, bool flag)
        {
            foreach (DictionaryEntry entry in userList)
            {
                TcpClient broadcaster = (TcpClient)entry.Value;
                NetworkStream broadcastStream = broadcaster.GetStream();
                Byte[] broadcastBuffer;
                /*if (flag)
                {
                    broadcastBuffer = System.Text.Encoding.ASCII.GetBytes(String.Format("{0}: {1}$", userName, message));
                }
                else
                {
                    broadcastBuffer = System.Text.Encoding.ASCII.GetBytes(message+"$");
                }*/
                
                broadcastBuffer = flag ? System.Text.Encoding.ASCII.GetBytes(String.Format("{0}: {1}$", userName, message)) : broadcastBuffer = System.Text.Encoding.ASCII.GetBytes(message+"$");
                broadcastStream.Write(broadcastBuffer, 0, broadcastBuffer.Length);
                broadcastStream.Flush();

                
            }
        }


    }
}
