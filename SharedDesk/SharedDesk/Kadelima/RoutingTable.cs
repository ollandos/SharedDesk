using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDesk
{
    [Serializable()]
    public class RoutingTable
    {
        private List<PeerInfo> table;
        private PeerInfo myInfo;

        public RoutingTable() 
        {
            table = new List<PeerInfo>();
            myInfo = new PeerInfo(666, "getMyIP", 666);
        }

        public PeerInfo findClosest(int targetGUID)
        {
            PeerInfo closest = null;
            int target = targetGUID;
            foreach (PeerInfo p in table)
            {
                if (closest == null)
                {
                    closest = p;
                }
                else if (calculateXOR(p.getGUID(), target) < calculateXOR(closest.getGUID(), target) )
                {
                    closest = p;
                }
            }
            return closest;
        }

        public PeerInfo findClosestFor(int senderGUID, int Target)
        {
            PeerInfo closest = myInfo;
            int target = Target;
            foreach (PeerInfo p in table)
            {
                if (calculateXOR(p.getGUID(), target) < calculateXOR(closest.getGUID(), target) && senderGUID != p.getGUID())
                {
                    closest = p;
                }
            }
            return closest;
        }

        public List<int> getTargetGUIDs(int guid)
        {
            List<int> targetGUIDs = new List<int>();
            double exponentCapacity = 4;
            for (double x = 0; x < exponentCapacity; x += 1d)
            {
                double result = Math.Pow(2, x);
                result = calculateXOR(guid, (int)result);
                targetGUIDs.Add((int)result);
            }
            return targetGUIDs;
        }

        public List<PeerInfo> getPeers() 
        {
            return table;
        }

        public int calculateXOR(int value1, int value2)
        {
            int result = 0;
            result = value1 ^ value2;
            return result;
        }

        public bool Contains(PeerInfo peer)
        {
            foreach (PeerInfo p in table)
            {
                if (p == peer)
                    return true;
            }
            return false;
        }

        public void Add(PeerInfo p) 
        {
            table.Add(p);
        }

        public int Count() 
        {
            return table.Count;
        }

        public string toString()
        {
            string result = "";
            int count = 0;
            foreach (PeerInfo p in this.table)
            {
                result += count + ": " + p.toString() + "\n";
                count++;
            }
            return result;
        }
    }
}
