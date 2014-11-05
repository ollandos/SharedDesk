﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDesk
{
    class RoutingTable
    {
        private List<PeerInfo> table;
        public RoutingTable() 
        {
            table = new List<PeerInfo>();
        }

        public PeerInfo findClosest(int senderGUID, int targetGUID)
        {
            PeerInfo closest = null;
            int target = targetGUID;
            foreach (PeerInfo p in table)
            {
                if (closest == null)
                {
                    closest = p;
                }
                else if (calculateXOR(p.getGUID(), target) < calculateXOR(closest.getGUID(), target) && senderGUID != -1)
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
    }
}
