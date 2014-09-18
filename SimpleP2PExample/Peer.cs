using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleP2PExample
{
    public class Peer
    {
        public string Id { get; private set; }

        public IPing Channel; //Outgoing communication
        public IPing Host;    //Incoming communication

        private DuplexChannelFactory<IPing> _factory;

        private readonly AutoResetEvent _stopFlag = new AutoResetEvent(false);

        public Peer(string id) {
            Id = id;
        }

        public void StartService() {

            var binding = new NetPeerTcpBinding();
            binding.Security.Mode = SecurityMode.None;

            var endpoint = new ServiceEndpoint(
                ContractDescription.GetContract(typeof(IPing)),
                binding,
                new EndpointAddress("net.p2p://SimpleP2P")
                );

            Host = new PingImplementation();

            _factory = new DuplexChannelFactory<IPing>(
                new InstanceContext(Host),
                endpoint );

            var channel = _factory.CreateChannel();

            ((ICommunicationObject)channel).Open();

            Channel = channel;

        }

        public void StopService() {
            ((ICommunicationObject)Channel).Close();
            if (_factory != null) {
                _factory.Close();
            }
        }

        public void Run() {
            Console.WriteLine("[ starting Service]");
            StartService();
            
            Console.WriteLine("[ Service Started ]");
            _stopFlag.WaitOne();

            Console.WriteLine("[ Stopping Service ]");
            StopService();

            Console.WriteLine("[ Service Stopped ]");

        }

        public void Stop() {
            _stopFlag.Set();
        }

        

    }
}
