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

        private IPEndPoint endPoint;
        private Socket socket;

        public UDPResponder(IPEndPoint endPoint) 
        {
            this.endPoint = endPoint;

            // init socket
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        public void sendPing()
        {
            // ping
            byte[] sendBuffer = new byte[] { 1 };
            socket.SendTo(sendBuffer, endPoint);
        }

    }
}
