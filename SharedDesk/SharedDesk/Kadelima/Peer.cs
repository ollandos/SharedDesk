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
        PeerInfo bootPeer;
        UDPListener listener;
        int GUID;

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
            bootPeer = new PeerInfo(0, "127.0.0.1", 6666);
        }

        public void init(int guid, string ip, int port)
        {
            // Assign GUID
            GUID = guid;
            
            // Create Routing Table and adding boot peer
            routingTable = new RoutingTable(new PeerInfo(guid, ip, port));
            routingTable.Add(bootPeer);

            // Create UDP listen and add events
            listener = new UDPListener(port);
            subscribeToListener(listener);

            // Create end point
            IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse(bootPeer.getIP()), bootPeer.getPORT());
            
            // Sending routing table request
            UDPResponder responder = new UDPResponder(remotePoint, port);
            responder.sendRequestRoutingTable();
        }

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

        // Handling the find closest final result
        public void addPeerInfo(PeerInfo pInfo)
        {
            Boolean isDuplicated = mNewRoutingTable.Contains(pInfo);
            if (!isDuplicated && pInfo.getGUID != GUID)
            {
                mNewRoutingTable.Add(pInfo);
            }
        }

        public RoutingTable getRoutingTable
        {
            get { return this.routingTable; }
        }
    }
}
