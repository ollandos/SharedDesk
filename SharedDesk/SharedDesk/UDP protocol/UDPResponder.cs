using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SharedDesk.UDP_protocol
{
    /// <summary>
    /// Class for responding to UDP packets
    /// </summary>
    class UDPResponder
    {
        // remote ip and port to send to
        private IPEndPoint endPoint;
        private Socket socket;

        // port the remote peer can respond to
        private int listenPort;

        public UDPResponder(IPEndPoint remotePoint, int listenPort) 
        {
            this.endPoint = remotePoint;
            this.listenPort = listenPort;

            // init socket
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

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

        public void sendGUID(byte[] guid)
        {
            // byte indicating what type of packet it is
            byte[] commandByte = new byte[] { 2 };

            // buffer to send
            byte[] sendBuffer = combineBytes(commandByte, guid);

            socket.SendTo(sendBuffer, endPoint);
            Console.WriteLine("\nUDP Responder"); 
            Console.WriteLine("Sending guid to {0}", endPoint);

        }

        public void sendRoutingTableRequest()
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


        /// <summary>
        /// Combine byte arrays
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


    }
}
