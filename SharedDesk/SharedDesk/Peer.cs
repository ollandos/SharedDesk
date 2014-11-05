using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDesk
{
    class Peer
    {
        private List<PeerInfo> routingTable;
        PeerInfo bootPeer;
        int GUID;

        public Peer(int guid) {
            routingTable = new List<PeerInfo>();
            bootPeer = new PeerInfo(0, "192.168.1.x", 6666);
            routingTable.Add(bootPeer);
            GUID = guid;
        }

    }
}
