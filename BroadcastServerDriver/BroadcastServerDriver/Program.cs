using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BroadcastServer;

namespace BroadcastServerDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            BroadcastServer.BroadcastServer server = new BroadcastServer.BroadcastServer("input.txt");
            server.sendFile(args[0]);
        }
    }
}
