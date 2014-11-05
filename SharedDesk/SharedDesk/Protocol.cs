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
    class Protocol
    {
        private Socket socket = null;
        private byte[] buff = new byte[2048];

        public Protocol()
        {

            IPEndPoint ServerEndPoint = new IPEndPoint(IPAddress.Any, 8080);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(ServerEndPoint);
            socket.BeginReceive(buff, 0, buff.Length - 1, SocketFlags.None, new AsyncCallback(Listen), socket);
            Console.Write("Listening on port 8080");

            //IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            //EndPoint Remote = (EndPoint)(sender);
            //int recv = socket.ReceiveFrom(data, ref Remote);
            //Console.WriteLine("Message received from {0}:", Remote.ToString());
            //Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));

        }

        public void Listen(IAsyncResult ar)
        {

            int received = 0;
            Socket s = null;

            try
            {
                s = (Socket)ar.AsyncState; //Grab our socket's state from the ASync return handler.
                received = s.EndReceive(ar); //Tell our socket to stop receiving data because our buffer is full.
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

            Console.WriteLine(String.Format("received {0} bytes", received));

            byte firstByte = buff[0];
            if (firstByte == 0)
            {
                // error
                return;
            }

            //switch (firstByte)
            //{
            //    case 0:
            //// error

            //    default:
            //        break;
            //}


            // TDOO: 
            // take the first byte of the packet

            // Options: 
            // 0 - error
            // 1 - ping
            // 2 - routing table
            // 3 - find closest peer 
            // 4 - listen on port (for file transfer)

        }



    }
}
