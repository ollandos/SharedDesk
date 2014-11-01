using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPatternExample.Transfer_Strategies
{
    class TransferTCPv5 : ITransferStrategy
    {
        public void sendFile(string filePath, System.Net.IPEndPoint endPoint)
        {
            // starts a thread that does the file sending
            SendFileThreadTCPv5 sender = new SendFileThreadTCPv5(filePath, endPoint);
        }

        public void listenForFile(string filePath, System.Net.IPEndPoint remotePoint)
        {
            // starts a thread that listen for a connection and expect a file 
            // remotePoint has an ip ( remote ip that it listen for, can also be IPAddress.Any
            // remotePoint port is the port to listen on
            ReceiveFileTCPv4 listener = new ReceiveFileTCPv4(filePath, remotePoint);
        }

    }
}
