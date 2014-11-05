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
            List<int> targetGUIDs = getTargetGUIDs();
            foreach (int guid in targetGUIDs)
            {
                //Find closest peer in own table
                PeerInfo closest = routingTable.findClosestTest(guid);
                //Sets target
                int target = guid;
                
                PeerInfo prevClosest = null;
                while (closest.getGUID() != target && closest != prevClosest)
                {
                    prevClosest = closest;
                    //closest = net[closest].askForClosestPeer(GUID, closest, target);
                }
                //check duplicate
                //If the peer already exist on the table we ignore.
                //If it does not exist add it to the table, and contact peer so that the other peer can register this peer too.
                bool isDuplicated = newRoutingTable.Contains(closest);
                if (!isDuplicated && closest.getGUID() != GUID)
                {
                    newRoutingTable.Add(closest);
                }
                isDuplicated = newRoutingTable.Contains(prevClosest);
            }
            if (newRoutingTable.Count != 0)
            {
                routingTable = newRoutingTable;
            }
        }

        public int askForClosestPeer(int guid, int Closest, int Target)
        {
            int closest = GUID;
            int target = Target;
            foreach (int p in RoutingTable)
            {
                if (calculateXOR(p, target) < calculateXOR(closest, target) && guid != p)
                {
                    closest = p;
                }
            }
            return closest;
        }

        

        

        public List<int> getTargetGUIDs()
        {
            List<int> targetGUIDs = new List<int>();

            double exponentCapacity = 4;
            for (double x = 0; x < exponentCapacity; x += 1d)
            {
                double result = Math.Pow(2, x);
                result = calculateXOR(this.GUID, (int)result);
                targetGUIDs.Add((int)result);
            }

            return targetGUIDs;
        }
    }
}
