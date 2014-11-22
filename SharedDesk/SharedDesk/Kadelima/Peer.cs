﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedDesk.UDP_protocol;
using SharedDesk.Kadelima;
using System.Net;

namespace SharedDesk
{
    public class Peer
    {

        private RoutingTable routingTable;
        private PeerInfo bootPeer;
        private UDPListener listener;
        private int GUID;
        //private const int DEFAULT_LISTENING_PORT = 6666;

        /// <summary>
        /// EVENTS FOR FORM
        /// </summary>
        public event handlerUpdatedTable updateTable;
        public delegate void handlerUpdatedTable();

        private RoutingTable mNewRoutingTable;

        private List<SearchChannel> channels;

        public Peer()
        {
            channels = new List<SearchChannel>();
            bootPeer = new PeerInfo(0, "127.0.0.1", 8080);
        }

        public void init(int guid, string ip, int port)
        {
            // Create UDP listen and add events
            listener = new UDPListener(port);
            subscribeToListener(listener);
 
            // Assign GUID
            GUID = guid;
            
            // Create Routing Table and adding boot peer
            routingTable = new RoutingTable(new PeerInfo(guid, ip, port));
            routingTable.add(0, bootPeer);

            listener.setRoutingtable = routingTable;

            // Create end point
            IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse(bootPeer.getIP()), bootPeer.getPORT());
            
            // Sending routing table request
            UDPResponder responder = new UDPResponder(remotePoint, port);
            responder.sendRequestRoutingTable();
        }

        /*
        //Refresing/Creating the routingTable
        public void makeTable()
        {
            RoutingTable newRoutingTable = new RoutingTable(routingTable.getMyInfo);
            List<int> targetGUIDs = newRoutingTable.getTargetGUIDs(GUID);
            foreach (int guid in targetGUIDs)
            {
                //Find closest peer in own table
                PeerInfo closest = routingTable.findClosest(guid);
                //Sets target
                int target = guid;
                PeerInfo prevClosest = null;
                while (closest.getGUID != target && closest != prevClosest)
                {
                    prevClosest = closest;
                    //closest = net[closest].askForClosestPeer(GUID, target);
                }
                //If found PeerInfo does not exist add it to the table.
                bool isDuplicated = newRoutingTable.Contains(closest);
                if (!isDuplicated && closest.getGUID != GUID)
                {
                    newRoutingTable.Add(closest);
                }
            }
            if (newRoutingTable.Count() != 0)
            {
                routingTable = newRoutingTable;
            }
        }
         * */

        //JOINING THE NETWORK
        //Joining Peer will tell all peers in his table to add him.
        public void joinToTable()
        {
            foreach (PeerInfo p in routingTable.getPeers())
            {
                    //p.Value.acceptPeer(PeerInfo);
                //p.Value.acceptPeer(routingTable.get);

            }
        }

        /*
        //Accepting newly joined Peer
        public void acceptPeer(PeerInfo peer)
        {
            bool isDuplicated = routingTable.Contains(peer);

            if (!isDuplicated)
            {
                routingTable.Add(peer);
            }

        }
         * */

        //Responding with closest found Peer
        public PeerInfo askForClosestPeer(int senderGUID, int target)
        {
            return routingTable.findClosestFor(senderGUID, target);
        }

        //Searches for closest peers in the network
        private void searchTargetPeers()
        {
            mNewRoutingTable = new RoutingTable(routingTable.getMyInfo);
            List<int> targetGUIDs = mNewRoutingTable.getTargetGUIDs(GUID);
            foreach (int guid in targetGUIDs)
            {
                PeerInfo closest = routingTable.findClosest(guid);
                SearchChannel channel = new SearchChannel(this, guid);
                channel.onReceiveClosest(closest);
                channels.Add(channel);
            }
        }

        public int getGUID
        {
            get { return this.GUID; }
        }

        /// <summary>
        /// EVENT HANDLERS
        /// </summary>

        // Subscribe to UDP listener events
        public void subscribeToListener(UDPListener l)
        {
            l.receiveTableRequest += new UDPListener.handlerRequestTable(handleRequestTable);
            l.receiveTable += new UDPListener.handlerTable(handleTable);
            l.receiveClosest += new UDPListener.handlerFindChannel(handleReceiveClosest);
            l.receiveRequestClosest += new UDPListener.handlerRequestClosest(handleRequestClosest);
            l.receiveRequestJoin += new UDPListener.handlerRequestJoin(handleJoinRequest);
            l.receiveRequestLeave += new UDPListener.handlerRequestLeave(handleLeaveRequest);
        }

        // Handling the routing table request
        public void handleRequestClosest(IPEndPoint remotePoint, int sender, int target)
        {
            PeerInfo targetInfo = askForClosestPeer(sender, target);
            UDPResponder responder = new UDPResponder(remotePoint, remotePoint.Port);
            responder.sendClosest(target, targetInfo);
        }

        // Handling the received routing table
        public void handleTable(RoutingTable table)
        {
            this.routingTable = table;
            updateTable();
            searchTargetPeers();
        }

        // Handling the routing table request
        public void handleRequestTable(IPEndPoint remotePoint)
        {
            UDPResponder responder = new UDPResponder(remotePoint, remotePoint.Port);
            responder.sendRoutingTable(routingTable);
        }

        // Handling the routing table request
        public void handleJoinRequest(PeerInfo newPeer)
        {
            //what key should the new Peer have???
            routingTable.add(newPeer.getGUID, newPeer);
            //TOOD: think about cycle of deleting non-target guids
            // Probably we want to remove those based when we find closer GUID.
            //Every time we find the closer peer we discard the peer which was previously occupying the key.
            //And right before discarding, we send leave request to the peer which is about to be removed.
        }


        // Handling the routing table request
        public void handleLeaveRequest(int guid)
        {
            routingTable.remove(guid);
        }

        // Handling receive routing table request
        public void handleReceiveClosest(int guid, PeerInfo currentClosest)
        {
            foreach (SearchChannel c in channels) {
                if (guid == c.getTargetGUID()) 
                {
                    c.onReceiveClosest(currentClosest);
                }
            }
        }

        public void addPeerInfo(int targetGUID, PeerInfo pInfo)
        {
            Boolean isDuplicated = routingTable.containsValue(pInfo);
            if (!isDuplicated)
            {
                //check if the key is occupied
                Boolean hasSameKey = routingTable.containsKey(targetGUID);
                if (hasSameKey)
                {
                    PeerInfo removingPeer = routingTable.get(targetGUID);
                    IPEndPoint remotePoint = new IPEndPoint( IPAddress.Parse(removingPeer.getIP()), removingPeer.getPORT() );
                    //notify to the peer with the targetGUID
                    //UDPResponder responder = new UDPResponder(remotePoint, DEFAULT_LISTENING_PORT);

                    routingTable.replace(targetGUID, pInfo);
                }
                else {
                    routingTable.add(targetGUID, pInfo);
                }
            }
        }

        public RoutingTable getRoutingTable
        {
            get { return this.routingTable; }
        }
    }
}
