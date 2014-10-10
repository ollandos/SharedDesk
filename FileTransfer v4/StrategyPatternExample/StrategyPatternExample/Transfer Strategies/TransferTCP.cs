using StrategyPatternExample.Transfer_Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyPatternExample
{
    class TransferTCP : ITransferStrategy
    {

        public override void send(string file)
        {
            Console.WriteLine("transfer using TCP");
        }

        public override void sendFile(string filePath, IPEndPoint endPoint)
        {
	    // TODO: 
	    // send in seperate thread
	    // add events that show progress, file complete, error etc
	    // 

            // Create a TCP socket.
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.SendBufferSize = 8192;


            // Connect the socket to the remote endpoint.
            client.Connect(endPoint);

            // Send file fileName to remote device
            //Console.WriteLine("Sending {0} to the host.", filePath);

	    // TODO: 
	    // use preBuffer and postBuffer to send filename, size, etc
	    // use postBuffer to send MD5 hash of file or something like that 
            // also look at SocketFlags http://msdn.microsoft.com/en-us/library/system.net.sockets.socketflags%28v=vs.110%29.aspx

            client.SendFile(filePath);

            // Release the socket.
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        public override void listenForFile(string filePath, IPEndPoint remotePoint)
        {
            ReceiveFileTCP listener = new ReceiveFileTCP(filePath, remotePoint);

	    // TODO
	    // Check if port ever closes
        }



    }
}
