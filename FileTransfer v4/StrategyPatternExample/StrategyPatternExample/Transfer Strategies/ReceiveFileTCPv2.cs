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
	class ReceiveFileTCPv2
	{

		// start seperate thread
		Thread t1;

		// where to save file
		string receivePath = "";
		IPEndPoint remotePoint;

		bool isFirstPacket = true;
		string fileName = "";
		int fileNameLength = 0;

		public ReceiveFileTCPv2(string filePath, IPEndPoint remotePoint)
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

			// Don't allow another socket to bind to this port.
		// Should perhaps consider this one
			//listener.ExclusiveAddressUse = true;

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
						fileNameLength = 255;
					}

			// TODO: 
			// get file size or FileInfo object

			// TODO: 
			// start download counter, datetime, total filesize to download etc
			// get MD5 hash or other hash of file
			// make sure all meta data and other stuff is actually sent
			// calculate hash of when file has been received and written to a file here
			// close socket and stop thread when entire file has been written


					fileName = Encoding.ASCII.GetString(tempState.buffer, 1, fileNameLength);
					receivePath += "\\" + fileName;

				}
				catch (Exception error)
				{
					Console.WriteLine(error.Message);
					receivePath += "\\" + "test.dat";
				}

			}

			BinaryWriter writer = null;
			try
			{

		// TODO: 
		// double check that file path is correct

				writer = new BinaryWriter(File.Open(receivePath, FileMode.Append));

				if (isFirstPacket)
				{
					// the first packet contain information that should not be written to the file itself so
					// if first packet then increase the index to size of fileName + one byte
			// since we increase the index, we need to reduce the count by the same amount
					int shift = fileNameLength + 1;
					writer.Write(tempState.buffer, shift, bytesRead - shift);
					isFirstPacket = false;

			// When you set the fileName 
					FileTransferEvents.FileReceived = fileName;

				}
				else
				{
					writer.Write(tempState.buffer, 0, bytesRead);

				}

		// TODO:
		// for each byte that has been read, update download counter
		// based on time and bytes received both mb/s and percentage downloaded
		// should be calculated in real time
		// create event

		// TODO: 
		// check if all bytes has been read and transfer is complete


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
