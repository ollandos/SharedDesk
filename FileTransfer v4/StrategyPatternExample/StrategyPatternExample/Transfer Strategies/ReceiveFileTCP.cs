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
    class ReceiveFileTCP
    {

        //int flag = 0;

        // start seperate thread
        Thread t1;

        // where to save file
        string receivePath = "";
        IPEndPoint remotePoint;

        //delegate void MyDelegate();

        public ReceiveFileTCP(string filePath, IPEndPoint remotePoint)
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

            public const int BufferSize = 8192;
            // Receive buffer.
            public byte[] buffer = new byte[BufferSize];
        }

        /// <summary>
        /// Notify thread to stop listening 
        /// </summary>
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public void StartListening()
        {

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.ReceiveBufferSize = StateObject.BufferSize;

            try
            {

                listener.Bind(this.remotePoint);
                listener.Listen(100);

                while (true)
                {
                    allDone.Reset();
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    allDone.WaitOne();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

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

            //if (flag == 0)
            //{
            //    fileNameLen = BitConverter.ToInt32(state.buffer, 0);
            //    string fileName = Encoding.UTF8.GetString(state.buffer, 4, fileNameLen);
            //    //receivedPath = @'C:\Users\Name\Desktop\' + fileName;
            //    flag++;
            //}
            //if (flag >= 1)
            //{
            //BinaryWriter writer = new BinaryWriter(File.Open(receivePath, FileMode.Append));
            //if (flag == 1)
            //{
            //    writer.Write(state.buffer, 4 + fileNameLen, bytesRead - (4 + fileNameLen));
            //    flag++;
            //}
            //else
            //{
            //    writer.Write(state.buffer, 0, bytesRead);
            //}
            //writer.Close();
            //handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            //}


            BinaryWriter writer = null;
            try
            {
                writer = new BinaryWriter(File.Open(receivePath, FileMode.Append));
                writer.Write(tempState.buffer, 0, bytesRead);
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

                // this method starts a new  AsyncCallback(ReadCallback)
                // and this method is ReadCallback so it works as a recursive method
                handler.BeginReceive(tempState.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), tempState);

                //Thread.CurrentThread.Interrupt();
            }


        }

    }
}
