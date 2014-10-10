using StrategyPatternExample.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPatternExample
{
    abstract class ITransferStrategy
    {

        public abstract void sendFile(string filePath, IPEndPoint endPoint);

        public abstract void listenForFile(string filePath, IPEndPoint remotePoint);

        public abstract void send(string filePath);

    }
}
