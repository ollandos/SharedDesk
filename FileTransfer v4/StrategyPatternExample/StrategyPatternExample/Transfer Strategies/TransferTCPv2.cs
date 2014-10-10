using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPatternExample.Transfer_Strategies
{
    class TransferTCPv2 : ITransferStrategy
    {
        public override void sendFile(string filePath, System.Net.IPEndPoint endPoint)
        {
            // TODO: 
            // send in seperate thread
            // add events that show progress, file complete, error etc

            // Create a TCP socket.
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.SendBufferSize = 8192;
	
            // TODO: 
	    // more options for Socket...

            // Connect the socket to the remote endpoint.
            client.Connect(endPoint);

            // TODO: 
            // use preBuffer and postBuffer to send filename, size, etc
            // use postBuffer to send MD5 hash of file or something like that 
            // also look at SocketFlags http://msdn.microsoft.com/en-us/library/system.net.sockets.socketflags%28v=vs.110%29.aspx

            byte[] fileName = Encoding.ASCII.GetBytes(Path.GetFileName(filePath));
            byte[] preBuffer = new byte[fileName.Length + 1];

	    // copy byte representing the size of the filename
	    // to the beginning of the first packet sent
            int length = fileName.Length;
            byte lengthB = (byte)length;
            new byte[] { lengthB }.CopyTo(preBuffer, 0);

	    // could format the transfer like this: 
            // 0x01 = filename
	    // then a byte representing size of that information (filename can be 0 - 255 characters ie. 

            // copies file name in ASCII format from index 1 to length of filename
            Array.Copy(fileName, 0, preBuffer, 1, fileName.Length);

	    // TODO: 
	    // Send FileInfo object of the file
	    // this will include timestamps, metadata, attributes etc 
	    // as well as file size

	    // FileInfo also have a build in replace method for creating a new file and then 
	    // taking a content of another file (the temp file of a file transfer) and add it to a new file

	    // sends size of filename (0-255) + filename and then disconnect after file has been queued for transmission
            client.SendFile(filePath, preBuffer, null, TransmitFileOptions.Disconnect);

            // Release the socket.
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        public override void listenForFile(string filePath, System.Net.IPEndPoint remotePoint)
        {
            ReceiveFileTCPv2 listener = new ReceiveFileTCPv2(filePath, remotePoint);
        }

        public override void send(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
