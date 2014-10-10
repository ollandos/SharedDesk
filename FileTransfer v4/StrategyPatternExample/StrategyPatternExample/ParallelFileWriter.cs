using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPatternExample
{
    /// <summary>
    /// Class for writing to file with multiple threads
    /// When 
    /// </summary>
    public sealed class ParallelFileWriter : IDisposable
    {
        // maxQueueSize is the maximum number of buffers you want in the queue at once.
        // If this value is reached, any threads calling Write() will block until there's
        // room in the queue.
        private readonly Task _writerTask;
        private readonly BlockingCollection<byte[]> _queue;
        private readonly FileStream _stream;

        public ParallelFileWriter(string filename, int maxQueueSize)
        {
            _stream = new FileStream(filename, FileMode.Create);
            _queue = new BlockingCollection<byte[]>(maxQueueSize);
            _writerTask = Task.Run(() => writerTask());
        }

        public void Write(byte[] data)
        {
            _queue.Add(data);
        }

        public void Dispose()
        {
            _queue.CompleteAdding();
            _writerTask.Wait();
            _stream.Close();
        }

        private void writerTask()
        {
            foreach (var data in _queue.GetConsumingEnumerable())
            {
                //Console.WriteLine("Queue size = {0}", _queue.Count);
                try
                {
                    _stream.Write(data, 0, data.Length);
                }
                catch (Exception error)
                {
		    // TODO: 
		    // use events to "report" errors
                    Console.WriteLine("Something went wrong writing to disk: " + error.Message);
                }
            }
        }

    }
}
