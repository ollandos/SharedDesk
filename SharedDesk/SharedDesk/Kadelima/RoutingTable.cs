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
        private Dictionary<int, PeerInfo> table;
        private PeerInfo myInfo;

        public RoutingTable(PeerInfo myInfo) 
        {
            table = new Dictionary<int, PeerInfo>();
            this.myInfo = myInfo;
        }

        public PeerInfo get(int targetGUID)
        {
            return table[targetGUID];
        }


        public PeerInfo findClosest(int targetGUID)
        {
            PeerInfo closest = null;
            int target = targetGUID;
            foreach (KeyValuePair<int, PeerInfo> entry in table)
            {
                PeerInfo p = entry.Value;
                if (closest == null)
                {
                    closest = p;
                }
                else if (calculateXOR(p.getGUID, target) < calculateXOR(closest.getGUID, target) )
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
            foreach (KeyValuePair<int, PeerInfo> entry in table)
            {
                PeerInfo p = entry.Value;
                if (calculateXOR(p.getGUID, target) < calculateXOR(closest.getGUID, target) && senderGUID != p.getGUID)
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

        public int calculateXOR(int value1, int value2)
        {
            int result = 0;
            result = value1 ^ value2;
            return result;
        }

        public List<PeerInfo> getPeers()
        {
            return table.Values.ToList();
        }

        public bool containsValue(PeerInfo peer)
        {
            return table.ContainsValue(peer);
        }

        public bool containsKey(int targetGuid)
        {
            return table.ContainsKey(targetGuid);
        }

        public void add(int targetGuid, PeerInfo p) 
        {
            table.Add(targetGuid, p);
        }

        public void replace(int targetGuid, PeerInfo p)
        {
            table.Remove(targetGuid);
            table.Add(targetGuid, p);
        }

        public void remove(int guid)
        {
            table.Remove(guid);
        }

        public int Count() 
        {
            return table.Count;
        }

        public PeerInfo getMyInfo
        {
            get { return myInfo; }
        }

        public string toString()
        {
            string result = "";
            int count = 0;
            foreach (KeyValuePair<int, PeerInfo> entry in table)
            {
                PeerInfo p = entry.Value;
                result += count + ": " + p.toString + "\n";
                count++;
            }
            return result;
        }
    }
}
