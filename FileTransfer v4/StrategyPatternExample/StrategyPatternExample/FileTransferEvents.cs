using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPatternExample
{

    public delegate void MessageHandler(ChangedEvent e);
    public class ChangedEvent : EventArgs
    {
        public ChangedEvent(object g)
        {
            Message = g;
        }
        public object Message { get; set; }
    }

    static class FileTransferEvents
    {
        // event handler
        public delegate void MessageHandler(ChangedEvent e);

        // events
        public static event MessageHandler transferStarted;
        public static event MessageHandler downloadComplete;
        public static event MessageHandler progress;
        public static event MessageHandler speedChange;

        public static string filename = "";
        public static string speed = "";
        public static byte percent = 0;

        // trigger event download complete with file name
        // when you change the static FileReceived variable
        public static string FileReceived
        {
            set { 
                filename = value;
                downloadComplete.Invoke(new ChangedEvent(value));
            }
        }

        public static string TransferStarted
        {
            set { 
                filename = value;
                transferStarted.Invoke(new ChangedEvent(value));
            }
        }

        public static byte Percentage
        {
            set {
                percent = value; 
                progress.Invoke(new ChangedEvent(value)); 
            }
        }

        public static string transferSpeed 
        {
            set {
                speed = value;
                speedChange.Invoke(new ChangedEvent(value)); 
            }
        }


    }
}
