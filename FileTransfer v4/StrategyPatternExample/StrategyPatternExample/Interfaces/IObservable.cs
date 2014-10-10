using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPatternExample.Strategy
{
    interface IObservable
    {
        void notify(string filePath, IPEndPoint endPoint, bool sending, bool transferComplete);
    }
}
