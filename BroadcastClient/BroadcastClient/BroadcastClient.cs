using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
namespace BroadcastClient
{
    public class BroadcastClient
    {
        private String[] dataStrings;
        String fileName = "output.txt";
        /// <summary>
        /// Creates the broadcastClient, with the filename to be outputed as the parameters.
        /// </summary>
        /// <param name="file"></param>
        public BroadcastClient(String file)
        {
            fileName = file;


        }
        /// <summary>
        /// Writes the data to file. If a packet was dropped, outputs the packet number.
        /// </summary>
        public void writeToFile()
        {
            for (int i = 0; i < dataStrings.Length; i++)
            {
                if (dataStrings[i] != null)
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true))
                    {
                        file.WriteLine(dataStrings[i]);
                    }
                }
                else
                {
                    Console.WriteLine("Packet #" + i + " missing!");
                }
            }

        }
        /// <summary>
        /// Starts the broadcastClient listening to the port number in the parameters.
        /// </summary>
        /// <param name="arg"></param>
        public void startListening(int PortNum)
        {
            int recv;
            byte[] data = new byte[1024];
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, PortNum);
            Socket newsock = new Socket(AddressFamily.InterNetwork,
                            SocketType.Dgram, ProtocolType.Udp);
            newsock.Bind(ipep);

            Console.WriteLine("Waiting for Broadcast.");
            // Set up response socket
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)(sender);
            int numOfPackets = 1;
            int currentPacket = 0;
            dataStrings = new String[1];



            while (currentPacket < numOfPackets)
            {
                data = new byte[2024];
                SocketFlags flg = new SocketFlags();
                recv = newsock.ReceiveFrom(data, data.Length, flg, ref Remote);
                Console.WriteLine("Recieved a packet.");
                // get sequenceNumber which is held in the first 2 bytes
                BroadcastPacket packet = new BroadcastPacket();
                packet.convertToPacket(data);

                if (packet.getPacketNumber() == 0)
                {
                    
                   // the rest of the data is just the number of packets
                    // the number is grabed and converted to int
                    numOfPackets = BitConverter.ToInt32(data, 4);
                    // array is resized to be the number of pieces of data
                    dataStrings = new String[numOfPackets+1];
                }
                else
                {
                    // convert the rest of the data to ascii text
                    dataStrings[packet.getPacketNumber()] = Encoding.ASCII.GetString(packet.getPacketData());
                    Console.WriteLine(Encoding.ASCII.GetString(packet.getPacketData()));

                }
                currentPacket = packet.getPacketNumber();

                // Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));

            }

        }
    }
}
