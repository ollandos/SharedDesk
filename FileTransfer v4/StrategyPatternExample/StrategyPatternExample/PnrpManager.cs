using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Net.PeerToPeer;

namespace StrategyPatternExample
{
    class PnrpManager
    {
        private readonly AutoResetEvent _stopFlag = new AutoResetEvent(false);
        PeerNameRegistration mRegistration;

        private const String IP_v4 = "IPv4";
        private const String IP_v6 = "IPv6";

        public PeerNameRecordCollection getPeerNameRecords(string classifier)
        {
            PeerNameRecordCollection peers = null;
            PeerNameResolver resolver = new PeerNameResolver();

            Console.WriteLine("Please wait, Resolving...");

            try
            {
                peers = resolver.Resolve(new PeerName(classifier, PeerNameType.Unsecured));

            }
            catch(PeerToPeerException ex)
            {
                Console.WriteLine("PeerToPeer Exception: {0}", ex.Message);
            }

            return peers;
        }

        public string[] getIPv4AndPort(string classifier)
        {
            return getIPAndPort(classifier, IP_v4);
        }

        public string[] getIPv6AndPort(string classifier)
        {
            return getIPAndPort(classifier, IP_v6);
        }

        public string[] getIPAndPort(string classifier, string IPVersion)
        {
            string[] result = { "", "" };

            PeerNameRecordCollection records = getPeerNameRecords(classifier);

            foreach (PeerNameRecord record in records)
            {
                foreach (var endpoint in record.EndPointCollection)
                {
                    String address = endpoint.Address.ToString();
                    if (IPVersion == IP_v6)
                    {
                        if ((address.IndexOfAny(new char[] { ':' }) >= 0))
                        {
                            //It's IPv6
                            result[0] = address;
                            result[1] = endpoint.Port.ToString();
                        }
                    }
                    else
                    {
                        if ((address.IndexOfAny(new char[] { ':' }) < 0))
                        {
                            //It's IPv4
                            result[0] = address;
                            result[1] = endpoint.Port.ToString();
                        }
                    }
                }
            }
            return result;
        }

        public void registerPeerName(string classifier)
        {

            PeerName peerName = new PeerName(classifier, PeerNameType.Unsecured);

            using (PeerNameRegistration registration = new PeerNameRegistration(peerName, 8080))
            {
                string timestamp = string.Format("Peer Created at: {0}", DateTime.Now.ToShortTimeString());

                registration.Comment = timestamp;

                UnicodeEncoding encoder = new UnicodeEncoding();
                byte[] data = encoder.GetBytes(timestamp);
                registration.Data = data;
                try
                {
                    registration.Start();
                    mRegistration = registration;
                    _stopFlag.WaitOne();
                }
                catch (PeerToPeerException ex){
                    Console.WriteLine("PeerToPeer Exception: {0}", ex.Message);
                }
            }

        }

        public void removeRegistration()
        {
            _stopFlag.Set();
            mRegistration.Stop();
        }

    }
}
