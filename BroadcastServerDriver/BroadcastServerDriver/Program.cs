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
            if (args.Length > 0)
            {
                server.sendFile(args[0]);
            }
            else
            {
                server.sendFile("192.168.0.255");
            }
        }
    }
}
