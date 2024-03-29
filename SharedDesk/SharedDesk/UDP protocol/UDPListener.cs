﻿using SharedDesk.UDP_protocol;
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
    /// Class that will handle the UDP socket
    /// </summary>
    public class UDPListener
    {
        private Socket socket = null;
        private byte[] buff = new byte[8192];

        // ref remoteEndPoint
        private EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        private int listenPort;

        // routing table to deliver per request
        private byte[] routing_table;

        // define all the possible commands
        private enum commandByte { 
            error = 0, pingRequest, pingResponse, routingTableRequest, routingTableReceived, closestPeerRequest, 
            closestPeerResponse, join, leave, fileInfo, fileTransferRequest, fileTransferResponse, 
            sendFileRequest
        };

        /// <summary>
        /// EVENTS FOR PEER
        /// </summary>
        public event handlerRequestTable receiveRequestTable;
        public delegate void handlerRequestTable(IPEndPoint endpoint);

        public event handlerTable receiveTable;
        public delegate void handlerTable(RoutingTable table);

        public event handlerFindChannel receiveClosest;
        public delegate void handlerFindChannel(int guid, PeerInfo pInfo);

        public event handlerRequestClosest receiveRequestClosest;
        public delegate void handlerRequestClosest(IPEndPoint endpoint, int sender, int target);

        public event handlerRequestJoin receiveRequestJoin;
        public delegate void handlerRequestJoin(int tagetGuid, PeerInfo pInfo);

        public event handlerRequestLeave receiveRequestLeave;
        public delegate void handlerRequestLeave(int guid);

        public UDPListener(int port)
        {
            this.listenPort = port;

            // Listen for any IP
            IPEndPoint ServerEndPoint = new IPEndPoint(IPAddress.Any, port);

            // Init socket
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.socket.ReceiveBufferSize = 8192;
            this.socket.SendBufferSize = 8192;

            socket.Bind(ServerEndPoint);

            // Start listening
            socket.BeginReceiveFrom(buff, 0, buff.Length, SocketFlags.None, ref remoteEndPoint, new AsyncCallback(Listen), socket);
            Console.Write("Listening on port {0} ", port);

        }

        public void closeSocket() {

            socket.Close();
        }

        public void Listen(IAsyncResult ar)
        {
            int received = 0;
            Socket s = null;
            EndPoint remoteEnd = new IPEndPoint(IPAddress.Any, 0);

            try
            {
                s = (Socket)ar.AsyncState;
                received = s.EndReceiveFrom(ar, ref remoteEnd);

                Console.WriteLine("\nUDP Listner port: {0}", listenPort);
                Console.WriteLine(
                    "{0} bytes received from {1}",
                    received,
                    remoteEnd
                );
            }
            catch (SocketException socketError)
            {
                Console.WriteLine("Socket error: " + socketError.Message);
                return;
            }
            catch (Exception error)
            {
                Console.WriteLine("Error: " + error.Message);
                return;
            }

            if (received <= 0)
            {
                return;
            }

            // Options: 
            // 0 - error
            // 1 - ping
            // 2 - ping response (guid)
            // 3 - routing table request
            // 4 - routing table
            // 5 - find closest peer request
            // 6 - peer info object (response from find closest peer request)

            byte firstByte = buff[0];
            commandByte command = (commandByte)firstByte;

            switch (command)
            {
                case commandByte.error:
                    break;
                case commandByte.pingRequest:
                    // Packet should be exactly 5 bytes
                    if (received != 5)
                    {
                        Console.WriteLine("Ping packet recevied wrong size...");
                        break;
                    }
                    handlePing(remoteEnd);

                    break;
                case commandByte.pingResponse:
                    // Packet should be exactly 17 bytes
                    if (received != 17)
                    {
                        Console.WriteLine("GUID packet recevied wrong size...");
                        break;
                    }
                    handleGuid(remoteEnd);
                    break;
                case commandByte.routingTableRequest:
                    handleRequestTable(remoteEnd);
                    break;
                case commandByte.routingTableReceived:
                    handleTable(remoteEnd);
                    break;
                case commandByte.closestPeerRequest:
                    handleRequestClosest(remoteEnd);
                    break;
                case commandByte.closestPeerResponse:
                    handleClosest();
                    break;
                case commandByte.join:
                    handleJoin();
                    break;
                case commandByte.leave:
                    handleLeave();
                    break;
                case commandByte.fileInfo:
                    handleFileInfo(remoteEnd);
                    break;
                case commandByte.fileTransferRequest:
                    break;
                case commandByte.fileTransferResponse:
                    break;
                case commandByte.sendFileRequest:
                    break;
                default:
                    Console.WriteLine("Not a valid command...");
                    break;
            }

            socket.BeginReceiveFrom(buff, 0, buff.Length, SocketFlags.None, ref remoteEndPoint, new AsyncCallback(Listen), socket);
        }

        private void handleFileInfo(EndPoint remoteEnd)
        {
            // get UDP listening port from peer
            byte[] portByteArray = buff.Skip(1).Take(16).ToArray();
            int port = BitConverter.ToInt32(portByteArray, 0);

            Console.WriteLine("Received a file info from: {0}, listen port: {1}", remoteEnd, port);
            // gets the first byte
            byte[] fileNameLengthByte = new byte[1];
            fileNameLengthByte = buff.Skip(5).Take(1).ToArray();

            // first byte has a value 0 - 255
            int fileNameLength = Convert.ToInt32(fileNameLengthByte[0]);

            // a fileName cannot be more then 255 characters because a byte cannot have have a higher value...
            if (fileNameLength > 255)
            {
                return;
            }

            // Windows 1252 encoding 
            Encoding encoding1252 = Encoding.GetEncoding(1252);
            string fileName = encoding1252.GetString(buff, 6, fileNameLength);

            byte[] fileSizeB = buff.Skip(5).Skip(1 + fileNameLength).Take(8).ToArray();
            long fileSize = BitConverter.ToInt64(fileSizeB, 0);

            // get md5 hash
            // md5 hash is 16 bytes = 128 bit
            byte[] md5 = buff.Skip(5).Skip(1 + fileNameLength + 8).Take(16).ToArray();
            //string md5String = BitConverter.ToString(md5).Replace("-", "");
            string md5String = BitConverter.ToString(md5);
 
            Console.WriteLine("\nFile Info:");
            Console.WriteLine("Name: {0}", fileName);
            Console.WriteLine("Size: {0}", fileSize.ToString());
            Console.WriteLine("md5: {0}", md5String);

            // create ip end point from udp packet ip and listen port received
            //IPEndPoint remoteIpEndPoint = remoteEnd as IPEndPoint;
            //remoteIpEndPoint.Port = port;

            // respond to ping (send guid)
            //UDPResponder udpResponse = new UDPResponder(remoteIpEndPoint, port);
            //udpResponse.sendGUID(guid);
        }

        private void handleRequestClosest(EndPoint remoteEnd)
        {
            int port = BitConverter.ToInt32(buff, 3);

            //Take the 2 GUIDS out of the buff[]
            byte[] guidByteArray = buff.Skip(1).Take(2).ToArray();
            // Get the first
            int senderGuid = guidByteArray[0];
            // Get the second
            int targetGuid = guidByteArray[1];
            Console.WriteLine("Received a find closest to {0} request from {1}", targetGuid, senderGuid);

            // Create IP endpoint from udp packet
            IPEndPoint remoteIpEndPoint = remoteEnd as IPEndPoint;
            remoteIpEndPoint.Port = port;

            // Raising the event to handle the information
            receiveRequestClosest(remoteIpEndPoint, senderGuid, targetGuid);
        }

        private void handleGuid(EndPoint remoteEnd)
        {
            byte[] guidByteArray = buff.Skip(1).Take(16).ToArray();
            Guid remoteGuid = new Guid(guidByteArray);
            Console.WriteLine("Received a guid from: {0}, guid: {1}", remoteEnd, remoteGuid.ToString());
        }

        private void handlePing(EndPoint remoteEnd)
        {
            // byte[] portByteArray = buff.Skip(1).Take(16).ToArray();
            int port = BitConverter.ToInt32(buff, 1);
            Console.WriteLine("Received a ping from: {0}, listen port: {1}", remoteEnd, port);

            // create ip end point from udp packet ip and listen port received
            IPEndPoint remoteIpEndPoint = remoteEnd as IPEndPoint;
            remoteIpEndPoint.Port = port;

            // respond to ping (send guid)
            UDPResponder udpResponse = new UDPResponder(remoteIpEndPoint, port);
            //udpResponse.sendGUID(guid);
        }

        private void handleRequestTable(EndPoint remoteEnd)
        {
            int port = BitConverter.ToInt32(buff, 1);
            Console.WriteLine("Received a routing table request from: {0}, listen port: {1}", remoteEnd, port);

            // Create ip end point from udp packet ip and listen port received
            IPEndPoint remoteIpEndPoint = remoteEnd as IPEndPoint;
            remoteIpEndPoint.Port = port;

            //Fire the event for routing table request
            receiveRequestTable(remoteIpEndPoint);
        }

        private void handleTable(EndPoint remoteEnd)
        {
            byte[] data = buff.Skip(1).ToArray();
            RoutingTable routingTable = byteArrayToRoutingTable(data);
            Console.Write(routingTable.toString());
            receiveTable(routingTable);
        }

        private void handleClosest()
        {
            byte[] data = buff.Skip(2).ToArray();
            byte[] channelGUID = buff.Skip(1).Take(1).ToArray();
            PeerInfo pInfo = UDPResponder.ByteArrayToPeerInfo(data); 
            int targetGUID = (int)channelGUID[0];

            Console.WriteLine("Received a responsse for closest to {0}, with GUID: {1}", targetGUID, pInfo.getGUID);

            //get the corresponding channel
            receiveClosest(targetGUID, pInfo);
            
        }

        private void handleJoin()
        {
            byte[] target = buff.Skip(1).Take(1).ToArray();
            int targetGUID = target[0];

            byte[] data = buff.Skip(2).ToArray();
            PeerInfo pInfo = UDPResponder.ByteArrayToPeerInfo(data);
            Console.WriteLine("Received a join to table request from {0}", pInfo.getGUID);
            //add the pInfo with event
            receiveRequestJoin(targetGUID, pInfo);
        }

        private void handleLeave()
        {
            byte[] data = buff.Skip(1).ToArray();
            int leavingGUID = BitConverter.ToInt32(data, 0);
            //remove the guid with event
            receiveRequestLeave(leavingGUID);
        }

        public RoutingTable setRoutingtable
        {
            set { this.routing_table = routingTableToByteArray(value); }
        }

        public RoutingTable getRoutingtable
        {
            get { return byteArrayToRoutingTable(this.routing_table); }
        }

        private static byte[] routingTableToByteArray(RoutingTable rt)
        {

            if (rt == null)
            {
                // trying to convert a null object
                return null;
            }

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, rt);

            return ms.ToArray();
        }

        private static RoutingTable byteArrayToRoutingTable(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);

            RoutingTable routingTable = (RoutingTable)binForm.Deserialize(memStream);

            return routingTable;
        }
    }
}
