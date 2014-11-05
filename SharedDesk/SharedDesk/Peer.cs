using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDesk
{
    class Peer
    {
        private RoutingTable routingTable;
        PeerInfo bootPeer;
        int GUID;

        public Peer(int guid, PeerInfo bootInfo) {
            routingTable = new RoutingTable();
            bootPeer = new PeerInfo(0, "192.168.1.x", 6666);
            routingTable.Add(bootPeer);
            GUID = guid;
        }

        public void makeTable(Dictionary<int, Peer> net)
        {
            RoutingTable newRoutingTable = new RoutingTable();
            List<int> targetGUIDs = newRoutingTable.getTargetGUIDs(GUID);
            foreach (int guid in targetGUIDs)
            {
                //Find closest peer in own table
                PeerInfo closest = routingTable.findClosest(guid);
                //Sets target
                int target = guid;
                PeerInfo prevClosest = null;
                while (closest.getGUID() != target && closest != prevClosest)
                {
                    prevClosest = closest;
                    //closest = net[closest].askForClosestPeer(GUID, closest, target);
                }
                //If found PeerInfo does not exist add it to the table.
                bool isDuplicated = newRoutingTable.Contains(closest);
                if (!isDuplicated && closest.getGUID() != GUID)
                {
                    newRoutingTable.Add(closest);
                }
            }
            if (newRoutingTable.Count() != 0)
            {
                routingTable = newRoutingTable;
            }
        }

        public PeerInfo askForClosestPeer(int senderGUID, int target) 
        {
            return routingTable.findClosestFor(senderGUID, target);
        }
        
    }
}
