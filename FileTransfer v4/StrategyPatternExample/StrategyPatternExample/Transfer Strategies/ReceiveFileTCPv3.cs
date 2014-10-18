using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyPatternExample.Transfer_Strategies
{
    class ReceiveFileTCPv3
    {

        // start seperate thread
        Thread t1;

        // where to save file
        string receivePath = "";
        IPEndPoint remotePoint;

        bool isFirstPacket = true;
        string fileName = "";
        int fileNameLength = 0;

        // total size of file
        long fileSize = 0;

        // bytes written to file
        // plus filename and one byte
        long totalBytesReceived = 0;

        // fileName size (1 byte)
        // fileSize (8 bytes)
        // + filename
        // + file content
        long totalBytesToBeReceived = 0;

        // used to calclate download speed per second
        BandwidthCounter counter = new BandwidthCounter();

        // Parallel File Writer uses a thread pool to queue writing threads,
        // should enable extremely fast download/writing speeds
        ParallelFileWriter fileWriter;

        public ReceiveFileTCPv3(string filePath, IPEndPoint remotePoint)
        {
            this.receivePath = filePath;
            this.remotePoint = remotePoint;
            // start thread
            this.t1 = new Thread(new ThreadStart(StartListening));
            this.t1.Start();
        }

        public class StateObject
        {
            // Client socket.
            public Socket workSocket = null;

            //public const int BufferSize = 8192;
            public const int BufferSize = 32768;
            //public const int BufferSize = 65536;

            // Receive buffer.
            public byte[] buffer = new byte[BufferSize];
        }

        /// <summary>
        /// Notify thread to stop listening 
        /// </summary>
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public void StartListening()
        {

            //Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //listener.ReceiveBufferSize = StateObject.BufferSize;
            //listener.DontFragment = true;

            TcpListener listener2 = new TcpListener(remotePoint);
            listener2.AllowNatTraversal(true);

            // Don't allow another socket to bind to this port.
            // Should perhaps consider this one
            //listener.ExclusiveAddressUse = true;

            try
            {

                //listener.Bind(this.remotePoint);
                //listener.Listen(100);

                while (true)
                {
                    allDone.Reset();
                    //listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    listener2.BeginAcceptSocket(new AsyncCallback(AcceptCallback), listener2);
                    allDone.WaitOne();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Reset all information and wait for new file
        /// </summary>
        private void resetConnection()
        {
            isFirstPacket = true;
            fileName = "";
            fileNameLength = 0;
            fileSize = 0;
            totalBytesReceived = 0;
            totalBytesToBeReceived = 0;
        }


        public void AcceptCallback(IAsyncResult ar)
        {

            allDone.Set();

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            //flag = 0;
        }

        public void ReadCallback(IAsyncResult ar)
        {

            //int fileNameLen = 1;
            //String content = String.Empty;
            StateObject tempState = (StateObject)ar.AsyncState;
            Socket handler = tempState.workSocket;
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead <= 0)
            {
                return;
            }

            counter.AddBytes((uint)bytesRead);

            if (isFirstPacket)
            {
                try
                {

                    // gets the first byte
                    byte[] firstByte = new byte[1];
                    firstByte = tempState.buffer.Take(1).ToArray();

                    // first byte has a value 0 - 255
                    fileNameLength = Convert.ToInt32(firstByte[0]);

                    // a fileName cannot be more then 255 characters because a byte cannot have have a higher value...
                    if (fileNameLength > 255)
                    {
                        // filename is not valid...
                        // this should not happen, should somehow at least validate the first packet to ensure
                        // it contain the right information...
                        return;
                        //fileNameLength = 255;
                    }

                    // TODO: 
                    // check if fileName is valid
                    // if file already exist (conflict)

                    fileName = Encoding.ASCII.GetString(tempState.buffer, 1, fileNameLength);
                    //receivePath += "\\" + fileName;

                    // after the file name comes the size of the file 
                    // should be a long 
                    // that is 64 bit = 8 byte
                    byte[] fileSizeB = tempState.buffer.Skip(1 + fileNameLength).Take(8).ToArray();
                    fileSize = BitConverter.ToInt64(fileSizeB, 0);

                    // set total to be received
                    totalBytesToBeReceived = fileNameLength + fileSize + 9;

                    // TODO: 
                    // get FileInfo object

                    // TODO: 
                    // start download counter, datetime, total filesize to download etc
                    // get MD5 hash or other hash of file
                    // make sure all meta data and other stuff is actually sent
                    // calculate hash of when file has been received and written to a file here
                    // close socket and stop thread when entire file has been written

                    // start timer that will execute an event every 1 sec
                    // that shows mb/s kb/s etc
                    System.Timers.Timer timer = new System.Timers.Timer() { Interval = 1000, Enabled = true };
                    timer.Elapsed += timer_Elapsed;
                    timer.Start();

                }
                catch (InvalidCastException castError)
                {
                    // was not able to find file size
                    Console.WriteLine(castError.Message);
                    return;
                }
                catch (Exception error)
                {
                    Console.WriteLine(error.Message);
                    return;
                }

            }

            BinaryWriter writer = null;
            try
            {

                // TODO: 
                // double check that file path is correct

                // TODO:
                // only open file while it is not in use
                // wait until file is ready to be written to

                if (isFirstPacket)
                {
                    // old way
                    //writer = new BinaryWriter(File.Open(savePathAndFileName, FileMode.Append));

                    string savePathAndFileName = receivePath + "\\" + fileName;

                    // creates a thread pool using BlockingCollection 
                    // this will allow a queue of max 200 threads waiting to write to the file
                    // if more than 200 threads are created, they are blocked and wait until the '
                    // queue is free
                    fileWriter = new ParallelFileWriter(savePathAndFileName, 200);

                    // the first packet contain information that should not be written to the file itself so
                    // if first packet then increase the index to size of fileName + one byte
                    // since we increase the index, we need to reduce the count by the same amount

                    // + 1 byte for size of fileName byte
                    // + 8 bytes for 64 bit long with file size
                    // shift = 9 bytes + fileName
                    int shift = fileNameLength + 9;
                    //writer.Write(tempState.buffer, shift, bytesRead - shift);

                    byte[] data = new byte[bytesRead - shift];
                    Array.Copy(tempState.buffer, shift, data, 0, bytesRead - shift);

                    fileWriter.Write(data);

                    isFirstPacket = false;

                    // set fileName, but do not trigger event
                    FileTransferEvents.filename = fileName;

                }
                else
                {
                    byte[] data = new byte[bytesRead];
                    Array.Copy(tempState.buffer, 0, data, 0, bytesRead);

                    fileWriter.Write(data);

                    //writer.Write(tempState.buffer, 0, bytesRead);
                }

                // TODO:
                // for each byte that has been read, update download counter
                // based on time and bytes received both mb/s and percentage downloaded
                // should be calculated in real time
                // create event

                // add to written files 
                totalBytesReceived += bytesRead;

                // TODO: 
                // check if all bytes has been read and transfer is complete
                if (totalBytesReceived == totalBytesToBeReceived)
                {
                    // trigger file received event
                    FileTransferEvents.FileReceived = fileName;

                    // reset connection
                    // set everything back to default and wait for new file
                    resetConnection();
                }

            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                Thread.Sleep(30);
            }
            finally
            {

                if (writer != null)
                {
                    writer.Close();
                }



                // triggers event with long of bytes written
                //FileTransferEvents.BytesReceived = written;

                // this method starts a new  AsyncCallback(ReadCallback)
                // and this method is ReadCallback so it works as a recursive method
                handler.BeginReceive(tempState.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), tempState);

                //Thread.CurrentThread.Interrupt();
            }


        }


        // every 1 sec get speed per sec and send event
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            // trigger event with percentage increase 
            float af = (float)totalBytesReceived / (float)totalBytesToBeReceived;
            //progressbar equals rounded.
            byte tot = (byte)Math.Round(af * 100);
            if (tot < 100)
            {
                FileTransferEvents.Percentage = tot;
            }
            else
            {
                FileTransferEvents.Percentage = 100;
            }

            FileTransferEvents.transferSpeed = counter.GetPerSecond();
        }

    }
}
