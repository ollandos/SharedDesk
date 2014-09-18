using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleP2PExample
{
    public class PingImplementation : IPing
    {

        #region IPing Members

        public void Ping(string sender, string message)
        {
            Console.WriteLine("{0} says: {1} ", sender, message);

            Thread.Sleep(1000);
        }

        #endregion
    }
}
