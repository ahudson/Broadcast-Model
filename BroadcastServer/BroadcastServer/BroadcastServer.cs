using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using BroadcastClient;

namespace BroadcastServer
{
    /// <summary>
    /// The BroadcastServer class is the server object that allows you to send data over the local subnet in a broadcast
    /// </summary>
   public class BroadcastServer
    {
        private string fileName;
        public BroadcastServer(string file)
        {
            fileName = file;
        }
        /// <summary>
        /// This method sends the file specified when the BroadcastServer object was initiated over the local subnet
        /// </summary>
        public void sendFile(string ipNumber)
        {
            string[] args = new string[0];
            //Create socket and endpoint.
            Socket sock = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Dgram,
                ProtocolType.Udp);
            EndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipNumber),
                2000);
            SendData(sock, endPoint);

        }
        /// <summary>
        /// This function sends data to the socket using the endPoint specified in sendFile()
        /// </summary>
        /// <param name="sock"></param>
        /// <param name="endPoint"></param>
        private void SendData(Socket sock, EndPoint endPoint)
        {
            // read from file
            string[] lines = System.IO.File.ReadAllLines(fileName);
            // add packet number to lines
            string[] outputLines = new string[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                int lineNum = i + 1;
                outputLines[i] = lineNum.ToString();
                outputLines[i] += lines[i];
            }
            byte[][] allLines = new byte[lines.Length][];

            // send to byte arrays
            for (int i = 0; i < lines.Length; i++)
            {
                allLines[i] = System.Text.Encoding.ASCII.GetBytes(outputLines[i]);

            }
            //Ensure that the number of bits is below 1020, need to find a better solution.
            for (int i = 0; i < lines.Length; i++)
            {
                if (allLines[i].Length > 1020)
                {
                    Console.WriteLine("Error: line size greater than packet size at line: " + i);
                    Environment.Exit(1);
                }

            }
            // send start packet
            BroadcastPacket startPacket = new BroadcastPacket(allLines.Length);
            Console.WriteLine("Sending start packet.");
            sock.SendTo(startPacket.getPacket(), startPacket.getPacket().Length, SocketFlags.None, endPoint);
            Thread.Sleep(2000);
            for (int i = 0; i < allLines.Length; i++)
            {
                BroadcastPacket packet = new BroadcastPacket(i + 1, allLines[i]);

                Console.WriteLine("Sending data: " + i);
                sock.SendTo(packet.getPacket(), packet.getPacket().Length, SocketFlags.None, endPoint);
                Thread.Sleep(2000);
            }
        }
        /// <summary>
        /// This function either returns the given IP address parsed from the arguments, or the default local subnet
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IPAddress GetIPAddr(string[] args)
        {
            if (args.Length == 2)
                return IPAddress.Parse(args[0]);
            else
                return IPAddress.Parse("192.168.0.255");
        }
        /// <summary>
        /// This either parses the argument port number to an integer, or returns a default port number of 1000
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static int GetPort(string[] args)
        {
            if (args.Length == 2)
                return int.Parse(args[1]);
            else
                return 1000;
        }
    }
}
