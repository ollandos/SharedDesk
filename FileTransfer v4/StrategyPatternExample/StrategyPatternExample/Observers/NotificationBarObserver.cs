using StrategyPatternExample.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPatternExample.Observers
{
    class NotificationBarObserver : IObservable
    {

	// shows popups in the icon bar when transfer of file 
	// is complete. 
	// Need reference to the no

        public void notify(string filePath, System.Net.IPEndPoint endPoint, bool sending, bool transferComplete)
        {
            throw new NotImplementedException();
        }
    }
}
