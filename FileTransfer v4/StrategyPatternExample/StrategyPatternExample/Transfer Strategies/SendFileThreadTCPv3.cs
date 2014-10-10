using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyPatternExample.Transfer_Strategies
{

    public class StateObject
    {
        // Client socket.
        public Socket workSocket = null;

        public const int BufferSize = 8192;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
    }


    class SendFileThreadTCPv3
    {
        // start seperate thread
        Thread t1;

        // remote ip and port
        IPEndPoint endPoint;
        Socket sendingSocket;

        // file to send
        string filePath;

        // TODO: 
        // add events for updating speed and percentage using 
        // BandWIthCounter

        public SendFileThreadTCPv3(string filePath, IPEndPoint endPoint)
        {
            this.endPoint = endPoint;
            this.filePath = filePath;

            this.t1 = new Thread(new ThreadStart(startSending));
            this.t1.Start();
        }

        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public void startSending()
        {

            try
            {
                // Create a TCP socket.
                sendingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //sendingSocket.SendBufferSize = 8192;
                sendingSocket.SendBufferSize = 32768;
                //sendingSocket.SendBufferSize = 65536;

                // Don't allow another socket to bind to this port.
                // Should perhaps consider this one
                //listener.ExclusiveAddressUse = true;

                // Connect the socket to the remote endpoint.
                sendingSocket.Connect(endPoint);


                // TODO: 
                // use preBuffer and postBuffer to send filename, size, etc
                // use postBuffer to send MD5 hash of file or something like that 
                // also look at SocketFlags http://msdn.microsoft.com/en-us/library/system.net.sockets.socketflags%28v=vs.110%29.aspx

                FileInfo f = new FileInfo(filePath);

                byte[] fileName = Encoding.ASCII.GetBytes(f.Name);
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

                // copy fileSizeB to end of preBuffer
                byte[] preBuffer = new byte[fileName.Length + 1 + 8];
                filenameSizePlusFilename.CopyTo(preBuffer, 0);
                Array.Copy(fileSizeB, 0, preBuffer, fileName.Length + 1, 8);

                // format the transfer like this: 
                // byte = filename size
                // file name
                // long = file size in bytes, ulong is 64 bit so filesize could be limitless
                // file content



                // TODO: 
                // Send FileInfo object of the file
                // this will include timestamps, metadata, attributes etc 
                // as well as file size

                // FileInfo also have a build in replace method for creating a new file and then 
                // taking a content of another file (the temp file of a file transfer) and add it to a new file

                // sends size of filename (0-255) + filename and then disconnect after file has been queued for transmission
                sendingSocket.BeginSendFile(filePath, preBuffer, null, TransmitFileOptions.Disconnect, new AsyncCallback(FileSendCallback), sendingSocket);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

                // this method starts a new  AsyncCallback(ReadCallback)
                // and this method is ReadCallback so it works as a recursive method
                //handler.BeginReceive(tempState.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), tempState);

                //Thread.CurrentThread.Interrupt();
            }

        }

        /// <summary>
        /// This method is called when the file has been sent
        /// </summary>
        /// <param name="ar"></param>
        public void FileSendCallback(IAsyncResult ar)
        {
            if (sendingSocket == null)
            {
                return;
            }

            // Retrieve the socket from the state object.
            Socket client = (Socket)ar.AsyncState;

            try
            {
                // Complete sending the data to the remote device.
                client.EndSendFile(ar);
            }
            catch (Exception error)
            {
                Console.WriteLine("error: " + error.Message);
            }
            finally
            {
                // Release the socket.
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }

        }


    }
}
