using SharedDesk.UDP_protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SharedDesk
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
            Console.Write("Listening on port 8080");

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

                    //byte[] portByteArray = buff.Skip(1).Take(16).ToArray();
                    int port = BitConverter.ToInt32(buff, 1);
                    Console.WriteLine("Receved a ping from: {0}, listen port: {1}", remoteEnd, port);

                    // create ip end point from udp packet ip and listen port received
                    IPEndPoint remoteIpEndPoint = remoteEnd as IPEndPoint;
                    remoteIpEndPoint.Port = port;

                    // respond to ping (send guid)
                    UDPResponder udpResponse = new UDPResponder(remoteIpEndPoint, port);
                    udpResponse.sendGUID(guid);

                    break;
                case 2:
                    // GUID (ping response)
                    Console.WriteLine("Received a guid from: {0}, guid: xxx", remoteEnd);
                    break;
                case 3:
                    // routing table request
                    break;
                case 4:
                    // routing table
                    break;
                case 5:
                    // find closest peer
                    break;
                case 6:
                    // peer info object
                    break;
                case 7:
                    // file transfer
                    break;
                default:
                    Console.WriteLine("Not a valid command...");
                    return;
            }


            //socket.BeginReceive(buff, 0, buff.Length, SocketFlags.None, new AsyncCallback(Listen), socket);
            socket.BeginReceiveFrom(buff, 0, buff.Length, SocketFlags.None, ref remoteEndPoint, new AsyncCallback(Listen), socket);
        }



    }
}
