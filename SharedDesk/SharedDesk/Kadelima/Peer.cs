using System;
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
        // My PeerInfo
        private PeerInfo myInfo;
        // Boot PeerInfo
        private PeerInfo bootPeer;
        // My RoutingTable
        private RoutingTable routingTable;
        // The UDP Listener
        private UDPListener listener;
        // The SearchChannel list
        private List<SearchChannel> channels;

        // Event to update Form
        public event handlerUpdatedTable updateTable;
        public delegate void handlerUpdatedTable();

        // TODO: pass boot peer after retrieval of peers from servers
        public Peer()
        {
            channels = new List<SearchChannel>();
            bootPeer = new PeerInfo(0, "145.93.116.180", 8080);
        }

        public void init(int guid, string ip, int port)
        {
            myInfo = new PeerInfo(guid,ip,port);

            // Create UDP listen and add events
            listener = new UDPListener(port);
            subscribeToListener(listener);
            
            // Create Routing Table and adding boot peer
            routingTable = new RoutingTable(myInfo);
            routingTable.add(bootPeer);

            listener.setRoutingtable = routingTable;

            // Create EndPoint
            IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse(bootPeer.getIP()), bootPeer.getPORT());
            // Sending routing table request
            UDPResponder responder = new UDPResponder(remotePoint, port);
            responder.sendRequestRoutingTable();
        }

        // Responding with closest found Peer
        public PeerInfo askForClosestPeer(int senderGUID, int target)
        {
            return routingTable.findClosestFor(senderGUID, target);
        }

        // Finds closest GUIDs and searches for closest peers to them in the network
        private void searchTargetPeers()
        {
            List<int> targetGUIDs = routingTable.getTargetGUIDs(myInfo.getGUID);
            foreach (int guid in targetGUIDs)
            {
                PeerInfo closest = routingTable.findClosest(guid);
                if(closest != null && myInfo.getGUID != closest.getGUID){
                    //routingTable.remove(closest.getGUID);
                    SearchChannel channel = new SearchChannel(this, guid);
                    channel.onReceiveClosest(closest);
                    channels.Add(channel);
                }
            }
        }

        public void sendLeaveRequests() {
            List<PeerInfo> list = new  List<PeerInfo>(routingTable.getPeers().Values);
            foreach(PeerInfo p in list)
            {
                sendLeaveRequest(p);
            }
        }

        private void sendLeaveRequest(PeerInfo pInfo)
        {
            listener.closeSocket();
            IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse(pInfo.getIP()), pInfo.getPORT());
            UDPResponder responder = new UDPResponder(remotePoint, myInfo.getPORT());
            responder.sendRequestLeave( myInfo.getGUID );
        }

        public int getGUID
        {
            get { return myInfo.getGUID; }
        }

        /// <summary>
        /// EVENT HANDLERS
        /// </summary>

        // Subscribe to UDP listener events
        public void subscribeToListener(UDPListener l)
        {
            l.receiveRequestTable += new UDPListener.handlerRequestTable(handleRequestTable);
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
            this.routingTable.MyInfo = myInfo;
            searchTargetPeers();
            updateTable();
        }

        // Handling the routing table request
        public void handleRequestTable(IPEndPoint remotePoint)
        {
            UDPResponder responder = new UDPResponder(remotePoint, remotePoint.Port);
            responder.sendRoutingTable(routingTable);
        }

        // Handling the routing table request
        public void handleJoinRequest(int targetGuid, PeerInfo newPeer)
        {
            List<int> targets = routingTable.getTargetGUIDs(myInfo.getGUID);
            if (targets.Contains(newPeer.getGUID))
            {
                routingTable.add(newPeer);
            }
            else if (!routingTable.containsValue(newPeer))
            {
                routingTable.addIfCloser(newPeer);
            }
            routingTable.cleanTable(myInfo.getGUID);
            updateTable();
        }



        // Handling the routing table request
        public void handleLeaveRequest(int guid)
        {
            routingTable.remove(guid);
            searchTargetPeers();
            updateTable();
        }

        // Handling receive routing table request
        public void handleReceiveClosest(int guid, PeerInfo currentClosest)
        {
            routingTable.cleanTable(myInfo.getGUID);
            foreach (SearchChannel c in channels) {
                if (guid == c.getTargetGUID()) 
                {
                    c.onReceiveClosest(currentClosest);
                }
            }
        }

        public void addPeerInfo(PeerInfo pInfo)
        {
         
           List<int> targets = routingTable.getTargetGUIDs(myInfo.getGUID);
            if (targets.Contains(pInfo.getGUID))
            {
                routingTable.add(pInfo);
                IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse(pInfo.getIP()), pInfo.getPORT());
                UDPResponder responder = new UDPResponder(remotePoint, myInfo.getPORT());
                responder.sendRequestJoin(1, myInfo);
            }
            else if (!routingTable.containsValue(pInfo))
            {
                bool added = routingTable.addIfCloser(pInfo);
                if (added)
                {
                    IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse(pInfo.getIP()), pInfo.getPORT());
                    UDPResponder responder = new UDPResponder(remotePoint, myInfo.getPORT());
                    responder.sendRequestJoin(1, myInfo);
                }
            }
            routingTable.cleanTable(myInfo.getGUID);
            updateTable();
        }

        public RoutingTable getRoutingTable
        {
            get { return this.routingTable; }
        }
    }
}
