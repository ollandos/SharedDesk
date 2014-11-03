using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace StrategyPatternExample.Transfer_Strategies
{

    /// <summary>
    /// In order to send large files the 
    /// socket.BeginSendFile does not work. Need to manually load bytes to buffer and send. 
    /// 
    /// </summary>
    internal class SendFileThreadTCPv5
    {
        // start seperate thread
        private Thread t1;

        // remote ip and port
        private IPEndPoint endPoint;
        private Socket sendingSocket;

        private BandwidthCounter counter = null;
        private string filePath;
        private bool isFirstPacket = true;
        private ulong totalBytesSent = 0;
        private ulong totalBytesToBeSent = 0;

        // timer to check mb/s kb/s etc
        // start timer for each transfer
        System.Timers.Timer timer_calc_speed = null;

        // if connection fails, attempt to reconnect
        // attempt to reconnect every 30 sec for 5 minutes
        private int timeBetweenConnectionAttempts = 30000;
        private byte reConnectAttempts = 10;
        private byte reConnectCounter = 0;

        public SendFileThreadTCPv5(string filePath, IPEndPoint endPoint)
        {
            this.endPoint = endPoint;
            this.filePath = filePath;

            this.t1 = new Thread(new ThreadStart(sendFile));
            this.t1.Start();
        }

        private byte[] getPreBuffer()
        {
            // get md5 hash
            ChecksumCalc checksum = new ChecksumCalc();
            byte[] md5 = checksum.GetMD5Checksum(filePath);

            FileInfo f = new FileInfo(filePath);

            // Using windows 1252 encoding
            Encoding encoding1252 = Encoding.GetEncoding(1252);

            //byte[] fileName = Encoding.ASCII.GetBytes(f.Name);
            byte[] fileName = encoding1252.GetBytes(f.Name);
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

            return preBuffer;
        }

        public void sendFile()
        {

            BinaryReader bin = null;
            FileInfo file = null;

            // attempt to get read from file
            Console.WriteLine("Attempting to read from file...");
            try
            {
                // get file info
                file = new FileInfo(filePath);

                // open file
                bin = new BinaryReader(File.OpenRead(filePath));

            }
            catch (Exception error)
            {
                Console.WriteLine("Error reading file: " + error.Message);
                return;
            }

            // attempt to connect 
            Console.WriteLine("Attempting to connect...");
            try
            {
                // Create a TCP socket.
                sendingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //sendingSocket.SendBufferSize = 8192;
                sendingSocket.SendBufferSize = 10240;
                //sendingSocket.SendBufferSize = 65536;

                // Don't allow another socket to bind to this port.
                // Should perhaps consider this one
                //listener.ExclusiveAddressUse = true;

                // Connect the socket to the remote endpoint.
                sendingSocket.Connect(endPoint);
            }
            catch (SocketException socketError)
            {
                Console.WriteLine("Error connecting: " + socketError.Message);

                // cannot connect, attempt to reconnect
                // reconnect every x ms, 1000 ms = 1 sec

                // this method will call itself basically (recursive)
                attemptToReconnect();
                return;

            }
            catch (Exception error)
            {

                Console.WriteLine("Error: " + error.Message);
                return;
            }

            if (isFirstPacket)
            {
                // send prebuffer
                Console.WriteLine("Calculating md5 hash of file...");
                sendingSocket.Send(getPreBuffer());

            }

            Console.WriteLine("Sending file.");

            // trigger event
            FileTransferEvents.TransferStarted = file.Name;

            // set amount of bytes to be receieved
            totalBytesToBeSent = (ulong)file.Length;

            // start bandwith counter
            totalBytesSent = 0;
            counter = new BandwidthCounter();

            // start timer to trigger kb/s mb/s progress event
            timer_calc_speed = new System.Timers.Timer() { Interval = 1000, Enabled = true };
            timer_calc_speed.Elapsed += timer_calc_speed_Elapsed;
            timer_calc_speed.Start();

            // create buffer
            //byte[] buff = new byte[2048];
            byte[] buff = new byte[10240];

            // loop through file
            while (totalBytesSent < totalBytesToBeSent)
            {
                try
                {
                    //sending buff's length.
                    totalBytesSent += (ulong)buff.Length;

                    //p.Sock.Send(bin.ReadBytes(buff.Length));
                    sendingSocket.Send(bin.ReadBytes(buff.Length));

                    // add to counter
                    counter.AddBytes((uint)buff.Length);

                }
                catch (SocketException socketError)
                {
                    Console.WriteLine("Error connecting: " + socketError.Message);
                    //attemptToReconnect();

                    return;

                }
                catch (Exception error)
                {
                    Console.WriteLine("Error transfering: " + error.Message);

                    // close file
                    if (bin != null)
                    {
                        bin.Close();
                    }

                    resetConnection();
                    return;

                }
                finally
                {
                }

            }

            // close file
            if (bin != null)
            {
                bin.Close();
            }

            resetConnection();

            FileTransferEvents.Percentage = 100;
            Console.WriteLine("File sent");


        }

        private void attemptToReconnect()
        {
            // attempt to reconnect
            if (reConnectAttempts > reConnectCounter)
            {

                Console.WriteLine(String.Format("Will reconnect in {0} milli seconds, for the {1} time", timeBetweenConnectionAttempts, reConnectCounter));
                Thread.Sleep(timeBetweenConnectionAttempts);

                reConnectCounter++;
                sendFile();
                reConnectCounter = 0;
            }
            else
            {

                Console.WriteLine(String.Format("{0} attempts to reconnect without success, giving up.", reConnectCounter));
            }

        }

        /// <summary>
        /// Reset all information and wait for new file
        /// </summary>
        private void resetConnection()
        {
            //isFirstPacket = true;
            //filePath = "";
            //totalBytesReceived = 0;
            //totalBytesToBeReceived = 0;

            timer_calc_speed.Stop();
            timer_calc_speed = null;

            // close socket
            sendingSocket.Close();

        }

        void timer_calc_speed_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // trigger event with percentage increase 
            float af = (float)totalBytesSent / (float)totalBytesToBeSent;
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