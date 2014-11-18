using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedDesk.UDP_protocol;
using System.Net;

namespace SharedDesk
{
    class Peer
    {
        private RoutingTable routingTable;
        PeerInfo bootPeer;
        UDPListener listener;
        UDPResponder responder;
        int GUID;

        public Peer(int guid)
        {
            routingTable = new RoutingTable();
            bootPeer = new PeerInfo(0, "127.0.0.1", 6666);
            routingTable.Add(bootPeer);
            GUID = guid;

        }

        public void init()
        {
            // create end point
            listener = new UDPListener(6666, Guid.NewGuid().ToByteArray());
            listener.setRoutingtable = routingTable;
            subscribeToListener(listener);

            IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse(bootPeer.getIP()), bootPeer.getPORT());
            responder = new UDPResponder(remotePoint, 6666);
            responder.sendRoutingTableRequest();
        }

        //Refresing/Creating the routingTable
        public void makeTable()
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
                    //closest = net[closest].askForClosestPeer(GUID, target);
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

        //JOINING THE NETWORK
        //Joining Peer will tell all peers in his table to add him.
        public void joinToTable()
        {
            foreach (PeerInfo p in routingTable.getPeers())
            {
                //p.Value.acceptPeer(routingTable.get);
            }
        }

        //Accepting newly joined Peer
        public void acceptPeer(PeerInfo peer)
        {
            bool isDuplicated = routingTable.Contains(peer);

            if (!isDuplicated)
            {
                routingTable.Add(peer);
            }

        }

        //Responding with closest found Peer
        public PeerInfo askForClosestPeer(int senderGUID, int target)
        {
            return routingTable.findClosestFor(senderGUID, target);
        }

        public void acceptTable(RoutingTable table)
        {
            this.routingTable = table;
        }

        public void subscribeToListener(UDPListener l)
        {
            l.receiveTable += new UDPListener.handlerTable(acceptTable);
            //l.receiveClosest += new UDPListener.handlerClosest();
        }

    }
}
