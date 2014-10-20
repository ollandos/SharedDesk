using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPatternExample.Transfer_Strategies
{

    public class StateObject
    {
        // Client socket.
        public Socket workSocket = null;

        public const int BufferSize = 32768;

        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
    }
}
