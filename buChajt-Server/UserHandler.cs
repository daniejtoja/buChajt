using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace buChajt_Server
{
    class UserHandler
    {
        TcpClient user;
        string id;
        Hashtable currentUsers;

        public UserHandler(TcpClient usr, string usrId, Hashtable usrs)
        {
            this.user = usr;
            this.id = usrId;
            this.currentUsers = usrs;
            Thread userThread = new Thread(handleChatting);
            userThread.Start();
        }

        private void handleChatting()
        {
            int requestsCounter = 0;
            byte[] fromBuffer = new byte[100000];
            string fromData;
            string requestsCounterInString;

            while (true)
            {
                try
                {
                    requestsCounter++;
                    NetworkStream networkStream = user.GetStream();
                    networkStream.Read(fromBuffer, 0, user.ReceiveBufferSize);
                    fromData = System.Text.Encoding.ASCII.GetString(fromBuffer);
                    fromData = fromData.Substring(0, fromData.IndexOf("$"));
                    Console.WriteLine(String.Format("FROM:{0} RECEIVED:{1}", id, fromData));
                    requestsCounterInString = requestsCounter.ToString();
                    Program.broadcastMessage(fromData, id, true);
                } 
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }

        }



    }
}
