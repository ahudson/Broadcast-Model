using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BroadcastClient;
using System.Net;
using System.Net.Sockets;
namespace BroadcastClientDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            BroadcastClient.BroadcastClient client = new BroadcastClient.BroadcastClient("output.txt");
            client.startListening(2000);
            client.writeToFile();
            //byte[] data = new byte[1024];
            //IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 2000);
            //Socket newsock = new Socket(AddressFamily.InterNetwork,
            //                SocketType.Dgram, ProtocolType.Udp);
            //newsock.Bind(ipep);
            //IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            //EndPoint Remote = (EndPoint)(sender);
            //SocketFlags flg = new SocketFlags();
            //int recv = newsock.ReceiveFrom(data, data.Length, flg, ref Remote);
            //Console.WriteLine("Recieved a packet.");
            Console.ReadLine();

        }
    }
}
