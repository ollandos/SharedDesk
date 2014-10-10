using StrategyPatternExample.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPatternExample
{
    class TransferFile
    {
        private ITransferStrategy _transferStrategy;

        // add methods and attributes to support the observer pattern
        List<IObservable> observers = new List<IObservable>();
        public TransferFile(ITransferStrategy t)
        {
            this._transferStrategy = t;
        }

        // for observer interface
        public void attach(IObservable ob)
        {
            observers.Add(ob);
        }

        // for observer interface
        public void detach(IObservable ob)
        {
            observers.Remove(ob);
        }

        // for observer interface

        /// <summary>
        /// Notifies all observers
        /// </summary>
        /// <param name="sending">indicate if file is being sent or received</param>
        public void notify(string filePath, IPEndPoint endPoint, bool sending, bool transferComplete)
        {
            foreach (IObservable ob in observers)
            {
                ob.notify(filePath, endPoint, sending, transferComplete);
            }

        }

        public void sendFile(string filePath, IPEndPoint endPoint)
        {
            notify(filePath, endPoint, true, false);
            _transferStrategy.sendFile(filePath, endPoint);
            notify(filePath, endPoint, true, true);
        }

        public void listenForFile(string filePath, IPEndPoint remotePoint)
        {
            notify(filePath, remotePoint, false, false);
            _transferStrategy.listenForFile(filePath, remotePoint);
            notify(filePath, remotePoint, false, true);
        }


    }
}
