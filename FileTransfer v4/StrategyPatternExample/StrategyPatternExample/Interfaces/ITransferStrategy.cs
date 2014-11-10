﻿using StrategyPatternExample.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPatternExample
{
    interface ITransferStrategy
    {

        void sendFile(string filePath, IPEndPoint endPoint);

        void listenForFile(string filePath, IPEndPoint remotePoint);

    }
}
