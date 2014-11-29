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

        // Returns closest peer to target (from our table)
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

        // Returns closest peer to target (from our table) if sender is not closer (used for find closest requests from other peers)
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

        // Get the neighbours of the passed GUID
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

        // Remove unwanted peers - Requires owner peer GUID
        public void cleanTable(int guid)
        {
            List<int> targets = getTargetGUIDs(guid);
            List<int> list = new List<int>(table.Keys);
            // Loop through list
            foreach (int p in list)
            {
                if (!targets.Contains(p))
                {
                   table.Remove(p);
                }
            }
        }

        // Returns xor result of passed values
        public int calculateXOR(int value1, int value2)
        {
            int result = 0;
            result = value1 ^ value2;
            return result;
        }

        // Returns the table
        public Dictionary<int,PeerInfo> getPeers()
        {
            return table;
        }

        // Returns true if table contains passed PeerInfo object
        public bool containsValue(PeerInfo peer)
        {
            List<PeerInfo> list = new List<PeerInfo>(table.Values);
            // Loop through list
            foreach (PeerInfo p in list)
            {
                if (p.getGUID == peer.getGUID)
                {
                    return true;
                }
            }
            return false;
        }

        public bool containsKey(int targetGuid)
        {
            return table.ContainsKey(targetGuid);
        }

        public void add(PeerInfo p) 
        {
            if (containsValue(p))
            {
                remove(p.getGUID);
            }
            if (table.ContainsKey(p.getGUID))
            {
                replace(p.getGUID, p);
            }
            else
            {
                table.Add(p.getGUID, p);
            }
        }

        public bool addIfCloser(PeerInfo p)
        {
            List<int> targets = getTargetGUIDs(myInfo.getGUID);
            List<int> closerToIndex = new List<int>();
            bool added = false;
            foreach (int tar in targets)
            {
                if (table.ContainsKey(tar))
                {
                    PeerInfo current = table[tar];
                    if (calculateXOR(p.getGUID, tar) < calculateXOR(current.getGUID, tar))
                    {
                        closerToIndex.Add(tar);
                    }
                }
                else
                {
                    closerToIndex.Add(tar);
                }
            }

            int index = -1;
            int dist = -1;
            foreach (int closer in closerToIndex)
            {
                if (dist == -1 || calculateXOR(p.getGUID, closer) < dist)
                {
                    index = closer;
                    dist = calculateXOR(p.getGUID, closer);
                }
            }

            if (index != -1)
            {
                added = true;
                if (table.ContainsKey(index))
                {
                    replace(index, p);
                }
                else 
                {
                    table.Add(index, p);
                }
            }
            return added;
        }

        public void replace(int targetGuid, PeerInfo p)
        {
            if (!containsValue(p))
            {
                PeerInfo temp = table[targetGuid];
                table.Remove(targetGuid);
                table.Add(targetGuid, p);
                addIfCloser(temp);
            }
        }

        public void remove(int guid)
        {
            foreach (KeyValuePair<int, PeerInfo> entry in table)
            {
                PeerInfo p = entry.Value;
                if (p.getGUID == guid)
                {
                    table.Remove(entry.Key);
                    break;
                }
            }
        }

        public int Count() 
        {
            return table.Count;
        }

        public PeerInfo MyInfo
        {
            get { return myInfo; }
            set { myInfo = value; }
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
