using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace StrategyPatternExample.Transfer_Strategies
{
    internal class SendFileThreadTCPv4
    {
        // start seperate thread
        private Thread t1;

        // remote ip and port
        private IPEndPoint endPoint;

        private Socket sendingSocket;

        // file to send
        private string filePath;

        // TODO:
        // add events for updating speed and percentage using
        // BandWIthCounter

        public SendFileThreadTCPv4(string filePath, IPEndPoint endPoint)
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
                // look at SocketFlags http://msdn.microsoft.com/en-us/library/system.net.sockets.socketflags%28v=vs.110%29.aspx

                // get md5 hash
                ChecksumCalc checksum = new ChecksumCalc();
                byte[] md5 = checksum.GetMD5Checksum(filePath);

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

                // sends size of filename (0-255) + filename 
                // plus file size and md5 hash of file
                // and then disconnect after file has been queued for transmission
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