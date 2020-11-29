using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace buChajt_Server
{
    class ServerConstants
    {
        public static readonly Random random = new Random();
        public static readonly IPAddress serverAddress = IPAddress.Parse("127.0.0.1");
        public static readonly Int32 serverPort = 8080;
    }
}
