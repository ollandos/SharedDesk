using SharedDesk.UDP_protocol;
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
    class UDPListener
    {
        private Socket socket = null;
        private byte[] buff = new byte[2048];

        // The unique 128 bit GUID
        private byte[] guid;

        // ref remoteEndPoint
        private EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        private int listenPort;

        // routing table to deliver per request
        private byte[] routing_table;

        /// <summary>
        /// EVENTS
        /// </summary>
        public event handlerTableRequest receiveTableRequest;
        public delegate void handlerTableRequest(IPEndPoint endpoint);

        public event handlerTable receiveTable;
        public delegate void handlerTable(RoutingTable table);

        public event handlerClosest receiveClosest;
        public delegate void handlerClosest(Peer table);

        public UDPListener(int port, byte[] guid)
        {
            // set guid
            this.guid = guid;
            this.listenPort = port;

            // listen for any IP
            IPEndPoint ServerEndPoint = new IPEndPoint(IPAddress.Any, port);

            // init socket
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(ServerEndPoint);

            // start listening
            //socket.BeginReceive(buff, 0, buff.Length, SocketFlags.None, new AsyncCallback(Listen), socket);
            socket.BeginReceiveFrom(buff, 0, buff.Length, SocketFlags.None, ref remoteEndPoint, new AsyncCallback(Listen), socket);
            Console.Write("Listening on port {0} ", port);

        }

        public void Listen(IAsyncResult ar)
        {

            int received = 0;
            Socket s = null;
            //IPPacketInformation packetInfo;
            EndPoint remoteEnd = new IPEndPoint(IPAddress.Any, 0);
            //SocketFlags flags = SocketFlags.None;

            try
            {
                s = (Socket)ar.AsyncState;
                //received = s.EndReceiveMessageFrom(ar, ref flags, ref remoteEnd, out packetInfo);
                received = s.EndReceiveFrom(ar, ref remoteEnd);

                Console.WriteLine("\nUDP Listner port: {0}", listenPort);
                Console.WriteLine(
                    //"{0} bytes received from {1} to {2}",
                    "{0} bytes received from {1}",
                    received,
                    remoteEnd
                    //packetInfo.Address
                );

                //s = (Socket)ar.AsyncState; //Grab our socket's state from the ASync return handler.
                //received = s.EndReceive(ar); //Tell our socket to stop receiving data because our buffer is full.
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
            // 7 - file transfer

            byte firstByte = buff[0];
            switch (firstByte)
            {
                case 0:
                    // error
                    break;
                case 1:
                    // ping
                    // Packet should be exactly 5 bytes
                    if (received != 5)
                    {
                        Console.WriteLine("Ping packet recevied wrong size...");
                        return;
                    }

                    handlePing(remoteEnd);

                    break;
                case 2:
                    // GUID (ping response)
                    // Packet should be exactly 17 bytes
                    if (received != 17)
                    {
                        Console.WriteLine("GUID packet recevied wrong size...");
                        return;
                    }
                    handleGuid(remoteEnd);
                    break;
                case 3:
                    // Handles the UDP packet containing the routing table request
                    handleTableRequest(remoteEnd);
                    break;
                case 4:
                    // Handles the UDP packet containing the routing table
                    handleTable(remoteEnd);
                    break;
                case 5:
                    // Handles the UDP packet containing a find closest request
                    handleClosestRequest(remoteEnd);
                    break;
                case 6:
                    // Handles the UDP packet containing a find closest request
                    break;
                case 7:
                    // file transfer
                    break;
                default:
                    Console.WriteLine("Not a valid command...");
                    return;
            }
            socket.BeginReceiveFrom(buff, 0, buff.Length, SocketFlags.None, ref remoteEndPoint, new AsyncCallback(Listen), socket);
        }

        private void handleClosestRequest(EndPoint remoteEnd)
        {
            int port = BitConverter.ToInt32(buff, 1);
            Console.WriteLine("Received a ping from: {0}, listen port: {1}", remoteEnd, port);

            // create ip end point from udp packet ip and listen port received
            IPEndPoint remoteIpEndPoint = remoteEnd as IPEndPoint;
            remoteIpEndPoint.Port = port;

            // respond to ping (send guid)
            UDPResponder udpResponse = new UDPResponder(remoteIpEndPoint, port);
            //udpResponse.send(guid);
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
            udpResponse.sendGUID(guid);
        }

        private void handleTableRequest(EndPoint remoteEnd)
        {
            int port = BitConverter.ToInt32(buff, 1);
            Console.WriteLine("Received a routing table request from: {0}, listen port: {1}", remoteEnd, port);

            // create ip end point from udp packet ip and listen port received
            IPEndPoint remoteIpEndPoint = remoteEnd as IPEndPoint;
            remoteIpEndPoint.Port = port;

            receiveTableRequest(remoteIpEndPoint);
            //UDPResponder udpResponse = new UDPResponder(remoteIpEndPoint, remoteIpEndPoint.Port);
            //udpResponse.sendRoutingTable(routing_table);
        }

        private void handleTable(EndPoint remoteEnd)
        {
            byte[] data = buff.Skip(1).ToArray();
            RoutingTable routingTable = byteArrayToRoutingTable(data);
            Console.Write(routingTable.toString());
            receiveTable(routingTable);
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
