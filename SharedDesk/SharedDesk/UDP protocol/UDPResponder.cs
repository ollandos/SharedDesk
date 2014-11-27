using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SharedDesk.UDP_protocol
{
    /// <summary>
    /// Class for responding to UDP packets
    /// </summary>
    public class UDPResponder
    {
        // remote ip and port to send to
        private IPEndPoint endPoint;
        private Socket socket;

        // port the remote peer can respond to
        private int listenPort;

        //CONSTRUCTOR
        public UDPResponder(IPEndPoint remotePoint, int listenPort)
        {
            this.endPoint = remotePoint;
            this.listenPort = listenPort;

            // init socket
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.ReceiveBufferSize = 8192;
            socket.SendBufferSize = 8192;
        }

        /// <summary>
        /// RESPONSE FUNCTIONS
        /// </summary>
        //sends ping udp packet to the endPoint
        public void sendPing()
        {

            // byte indicating what type of packet it is
            byte[] commandByte = new byte[] { 1 };

            // byte array with port 
            byte[] listenPortByteArray = BitConverter.GetBytes(listenPort);

            // buffer to send
            byte[] sendBuffer = combineBytes(commandByte, listenPortByteArray);

            socket.SendTo(sendBuffer, endPoint);
            Console.WriteLine("\nUDP Responder");
            Console.WriteLine("Sending ping to {0}", endPoint);
        }


        public void sendGUID(int GUID)
        {
            byte[] guid = new byte[] { (byte)GUID };
            // byte indicating what type of packet it is
            byte[] commandByte = new byte[] { 2 };

            // buffer to send
            byte[] sendBuffer = combineBytes(commandByte, guid);

            socket.SendTo(sendBuffer, endPoint);
            Console.WriteLine("\nUDP Responder");
            Console.WriteLine("Sending guid to {0}", endPoint);
        }

        //sends a request to join the table of the peer at the endpoint
        public void sendRequestJoin(int targetGUID, PeerInfo myInfo)
        {
            // byte indicating what type of packet it is
            byte[] commandByte = new byte[] { 7 };

            //
            byte[] target = new byte[] { (byte)targetGUID };

            // byte array with port 
            byte[] peerInfoByteArray = peerInfoToByteArray(myInfo);

            // buffer to send
            byte[] sendBuffer = combineBytes(commandByte, target, peerInfoByteArray);

            socket.SendTo(sendBuffer, endPoint);
            Console.WriteLine("\nUDP Responder");
            Console.WriteLine("Sending request to join the table of peer {0}", endPoint);
        }

        //sends a request to join the table of the peer at the endpoint
        public void sendRequestLeave(int guid)
        {
            // byte indicating what type of packet it is
            byte[] commandByte = new byte[] { 8 };

            // byte array with port 
            byte[] guidByteArray = BitConverter.GetBytes(guid);

            // buffer to send
            byte[] sendBuffer = combineBytes(commandByte, guidByteArray);

            socket.SendTo(sendBuffer, endPoint);
            Console.WriteLine("\nUDP Responder");
            Console.WriteLine("Sending request to join the table of peer {0}", endPoint);
        }

        //sends a routing table request to the endpoint
        public void sendRequestRoutingTable()
        {
            // byte indicating what type of packet it is
            byte[] commandByte = new byte[] { 3 };

            // byte array with port 
            byte[] listenPortByteArray = BitConverter.GetBytes(listenPort);

            // buffer to send
            byte[] sendBuffer = combineBytes(commandByte, listenPortByteArray);

            socket.SendTo(sendBuffer, endPoint);
            Console.WriteLine("\nUDP Responder");
            Console.WriteLine("Sending routing table request to {0}", endPoint);
        }

        //sends the routing table to the endpoint
        public void sendRoutingTable(RoutingTable t)
        {
            // Convert RoutingTable to byte[]
            byte[] table = routingTableToByteArray(t);

            // byte indicating what type of packet it is
            byte[] commandByte = new byte[] { 4 };

            // buffer to send
            byte[] sendBuffer = combineBytes(commandByte, table);

            // Sending UPD packet
            socket.SendTo(sendBuffer, endPoint);
            Console.WriteLine("\nUDP Responder");
            Console.WriteLine("Sending routing table to {0}", endPoint);
        }

        //sends the closest found PeerInfo to the endpoint
        public void sendRequestClosest(int self, int target)
        {
            // Command byte
            byte[] commandByte = new byte[] { 5 };

            // Own Guid to byte[]
            byte[] own_guid = new byte[1] { (byte)self };
            // Target Guid to byte[]
            byte[] target_guid = new byte[1] { (byte)target };
            // Combining Guids
            byte[] guids = combineBytes(own_guid, target_guid);

            // Listen port to byte[] 
            byte[] listenPortByteArray = BitConverter.GetBytes(listenPort);

            // Creating buffer to send
            byte[] sendBuffer = combineBytes(commandByte, guids, listenPortByteArray);

            socket.SendTo(sendBuffer, endPoint);
            Console.WriteLine("\nUDP Responder");
            Console.WriteLine("Sending find closest to {0} request to {1}", target, endPoint);
        }

        //sends the closest found PeerInfo to the endpoint
        public void sendClosest(int targetGUID, PeerInfo closest)
        {
            // Convert PeerInfo to byte[]
            byte[] peerInfoInBytes = peerInfoToByteArray(closest);

            // byte indicating what type of packet it is
            byte[] commandByte = new byte[] { 6 };

            byte[] targetGUIDInBytes = new byte[] { (byte)targetGUID };

            // buffer to send
            byte[] tempBuffer = combineBytes(commandByte, targetGUIDInBytes);
            byte[] sendBuffer = combineBytes(tempBuffer, peerInfoInBytes);

            // Sending UPD packet
            socket.SendTo(sendBuffer, endPoint);
            Console.WriteLine("\nUDP Responder");
            Console.WriteLine("Sending closest found Peer {0} to {1}", closest.getGUID, targetGUID);
        }

        public void sendFileInfo(string filePath)
        {

            // check if file exists
            if (File.Exists(filePath) == false)
            {
                return;
            }

            // get md5 hash
            ChecksumCalc checksum = new ChecksumCalc();
            byte[] md5 = checksum.GetMD5Checksum(filePath);

            FileInfo f = new FileInfo(filePath);

            // Using windows 1252 encoding
            Encoding encoding1252 = Encoding.GetEncoding(1252);

            //byte[] fileName = Encoding.ASCII.GetBytes(f.Name);
            byte[] fileName = encoding1252.GetBytes(f.Name);
            byte[] filenameSizePlusFilename = new byte[fileName.Length + 1];

            // copy byte representing the size of the filename
            // to the beginning of the first packet sent
            int length = fileName.Length;
            byte lengthB = (byte)length;
            new byte[] { lengthB }.CopyTo(filenameSizePlusFilename, 0);

            // copies file name in ASCII format from index 1 to length of filename
            Array.Copy(fileName, 0, filenameSizePlusFilename, 1, fileName.Length);

            // get file size and convert to 8 bytes
            long fileSize = f.Length;
            byte[] fileSizeB = BitConverter.GetBytes(fileSize);

            // preBuffer has size:
            // file name +
            // 1 byte for file name size +
            // 8 bytes is the size of the file + 
            // 16 bytes is the md5 hash of the file 
            byte[] preBuffer = new byte[fileName.Length + 1 + 8 + 16];

            // copy file name size and file name to preBuffer
            filenameSizePlusFilename.CopyTo(preBuffer, 0);

            // copy fileSizeB to end of preBuffer
            Array.Copy(fileSizeB, 0, preBuffer, fileName.Length + 1, 8);

            // copy md5 hash to preBuffer
            Array.Copy(md5, 0, preBuffer, fileName.Length + 1 + 8, 16);

            // format the transfer like this:
            // byte = filename size
            // file name
            // long = file size in bytes, ulong is 64 bit so filesize could be limitless
            // 16 bytes md5 hash of file
            // file content

            // byte indicating what type of packet it is
            byte[] commandByte = new byte[] { 9 };

            // byte array with port 
            byte[] listenPortByteArray = BitConverter.GetBytes(listenPort);

            // Creating buffer to send
            byte[] sendBuffer = combineBytes(commandByte, listenPortByteArray, preBuffer);

            // Sending UPD packet
            socket.SendTo(sendBuffer, endPoint);
            Console.WriteLine("\nUDP Responder");
            Console.WriteLine(String.Format("Sending file info to {0}, file: \"{1}\"", endPoint.ToString(), f.Name));

        }

        /// <summary>
        /// Byte[] Functionality
        /// </summary>
        public byte[] combineBytes(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        public byte[] combineBytes(byte[] first, byte[] second, byte[] third)
        {
            byte[] ret = new byte[first.Length + second.Length + third.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            Buffer.BlockCopy(third, 0, ret, first.Length + second.Length,
                             third.Length);
            return ret;
        }

        private static byte[] routingTableToByteArray(RoutingTable rt)
        {

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, rt);

            return ms.ToArray();
        }

        public static byte[] peerInfoToByteArray(PeerInfo pi)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, pi);

            return ms.ToArray();
        }

        public static PeerInfo ByteArrayToPeerInfo(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);

            PeerInfo peerInfo = (PeerInfo)binForm.Deserialize(memStream);

            return peerInfo;
        }

    }
}
