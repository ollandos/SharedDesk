using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleP2PExample
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Count() <= 1) {

                for (int i = 0; i < 4; i++) {
                    Process.Start("SimpleP2PExample.exe");
                }

            }

            new Program().Run();
        }

        private void Run()
        {
            Console.WriteLine("Starting the simple P@P demo.");

            var peer = new Peer("Peer" + Guid.NewGuid()+")");
            var peerThread = new Thread(peer.Run) { IsBackground = true };
            peerThread.Start();

            Thread.Sleep(1000);

            while (true) {
                Console.Write("Enter Something:");
                string tmp = Console.ReadLine();

                if (tmp == "")
                {
                    break;
                }

                peer.Channel.Ping(peer.Id, tmp);
            
            }

            peer.Stop();
            peerThread.Join();

        }
    }
}
