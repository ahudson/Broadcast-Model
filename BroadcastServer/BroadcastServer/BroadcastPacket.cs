using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
namespace BroadcastServer
{

    public class BroadcastPacket
    {
        private int packetNum;
        private byte[] data;
        bool isStartPacket;
        int numOfPackets;
        /// <summary>
        /// Creates an empty broadcast packet
        /// </summary>
        public BroadcastPacket()
        {
            isStartPacket = false;
            numOfPackets = 0;
            data = new byte[0];

        }
        /// <summary>
        /// create a start packet
        /// </summary>
        /// <param name="newPacketNum"></param>
        public BroadcastPacket(int numPackets)
        {
            packetNum = 0;
            isStartPacket = true;
            numOfPackets = numPackets;
            data = new byte[0];
        }
        /// <summary>
        /// create regular data packet
        /// </summary>
        /// <param name="seqNum"></param>
        /// <param name="newData"></param>
        public BroadcastPacket(int seqNum, byte[] newData)
        {
            packetNum = seqNum;
            data = newData;
            numOfPackets = 0;
            isStartPacket = false;
        }

        public void setPacketNumber(int newPacketNum)
        {
            packetNum = newPacketNum;
        }
        public int getPacketNumber()
        {
            return packetNum;
        }

        public byte[] getPacketData()
        {
            if (isStartPacket == false)
            {
                return data;
            }
            else
            {
                return null;
            }
        }
        public void setPacketData(byte[] newData)
        {
            data = newData;
        }

        public bool getIfStartPacket()
        {
            return isStartPacket;
        }
        /// <summary>
        /// Returns a byte array to be sent through socket
        /// </summary>
        /// <returns></returns>
        public byte[] getPacket()
        {
            if (isStartPacket == false)
            {
                byte[] returnArray = new byte[data.Length + 4];
                byte[] byteSeqNum = BitConverter.GetBytes(packetNum);
                returnArray[0] = byteSeqNum[0];
                returnArray[1] = byteSeqNum[1];
                returnArray[2] = byteSeqNum[2];
                returnArray[3] = byteSeqNum[3];
                for (int i = 0; i < data.Length; i++)
                {
                    returnArray[i + 4] = data[i];
                }
                return returnArray;
            }
            else
            {
                byte[] returnArray = new byte[8];
                byte[] byteSeqNum = BitConverter.GetBytes(0);
                byte[] byteNumPacktes = BitConverter.GetBytes(numOfPackets);
                returnArray[0] = byteSeqNum[0];
                returnArray[1] = byteSeqNum[1];
                returnArray[2] = byteSeqNum[2];
                returnArray[3] = byteSeqNum[3];
                returnArray[4] = byteNumPacktes[0];
                returnArray[5] = byteNumPacktes[1];
                returnArray[6] = byteNumPacktes[2];
                returnArray[7] = byteNumPacktes[3];
                return returnArray;
            }
        }
        /// <summary>
        /// Takes in a byte array so it is possible to get the sequence number and packet data
        /// </summary>
        /// <param name="packetInfo"></param>
        public void convertToPacket(byte[] packetInfo)
        {
            Byte[] sequenceNumber = new Byte[4];
            sequenceNumber[0] = packetInfo[0];
            sequenceNumber[1] = packetInfo[1];
            sequenceNumber[2] = packetInfo[2];
            sequenceNumber[3] = packetInfo[3];
            // convert the sequence number to an int
            //string seqStr = BitConverter.ToString(sequenceNumber, 0);
            //packetNum = int.Parse(seqStr);
            packetNum = BitConverter.ToInt32(sequenceNumber, 0);
            // if start packet
            if (packetNum == 0)
            {
                isStartPacket = true;
                // the rest of the data is just the number of packets
                // the number is grabed and converted to int
                numOfPackets = BitConverter.ToInt32(packetInfo, 4);
                // array is resized to be the number of pieces of data
            }
            else
            {
                byte[] tempData = new byte[packetInfo.Length - 4];
                // convert the rest of the data to ascii text
                for (int i = 4; i < packetInfo.Length; i++)
                {
                    tempData[i - 4] = packetInfo[i];
                }
                data = tempData;
            }
        }
    }
}
