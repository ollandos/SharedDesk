using StrategyPatternExample.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPatternExample.Observers
{
    class ConsoleObserver : IObservable
    {

        public void notify(string filePath, System.Net.IPEndPoint endPoint, bool sending)
        {
            if (sending)
            {
                Console.WriteLine("*** SENDING FILE ***");
                Console.WriteLine(String.Format("TO IP: {0} : {1}", endPoint.Address.ToString(), endPoint.Port));
            }
            else
            {
                Console.WriteLine("*** RECEIVING FILE ***");
                Console.WriteLine(String.Format("FROM IP: {0} : {1}", endPoint.Address.ToString(), endPoint.Port));
            }

            Console.WriteLine(String.Format("FILE: {0}", filePath));
        }

        public void notify(string filePath, System.Net.IPEndPoint endPoint, bool sending, bool transferComplete)
        {
            if (sending)
            {
                if (transferComplete == false)
                {
                    Console.WriteLine("*** SENDING FILE ***");
                    Console.WriteLine(String.Format("TO IP: {0} : {1}", endPoint.Address.ToString(), endPoint.Port));
                }
                else
                {
                    //Console.WriteLine("*** SENDING FILE COMPLETE ***");
                    //Console.WriteLine(String.Format("TO IP: {0} : {1}", endPoint.Address.ToString(), endPoint.Port));
                }

                Console.WriteLine(String.Format("FILE: {0}", filePath));
            }
            else
            {

                if (transferComplete == false)
                {
                    Console.WriteLine("*** RECEIVING FILE ***");
                    Console.WriteLine(String.Format("FROM IP: {0} : {1}", endPoint.Address.ToString(), endPoint.Port));
                }
                else
                {
                    //Console.WriteLine("*** RECEIVING FILE COMPLETE***");
                    //Console.WriteLine(String.Format("FROM IP: {0} : {1}", endPoint.Address.ToString(), endPoint.Port));
                }

                Console.WriteLine(String.Format("FOLDER: {0}", filePath));

            }


        }
    }
}
