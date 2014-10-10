using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPatternExample
{
    class TransferUDP : ITransferStrategy
    {
        public override void send(string file)
        {
            Console.WriteLine("transfer using UDP");
        }

        public override void sendFile(string filePath, System.Net.IPEndPoint endPoint)
        {
            throw new NotImplementedException();
        }

        public override void listenForFile(string filePath, System.Net.IPEndPoint remotePoint)
        {
            throw new NotImplementedException();
        }
    }
}
