using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedDesk.UDP_protocol;
using SharedDesk.Kadelima;
using System.Net;
using System.Timers;
using System.Threading;

namespace SharedDesk
{
    public class Peer
    {
        // Wait time for refreshing the table in milisec
        private static int REFRESH_TIME = 30000;
        // Wait time for pinging closests peers in milisec
        private static int PING_INTERVALS = 8000;
        // Wait time for response for a ping request in milisec
        private static int WAIT_TIME_FOR_PING_RESPONSE = 5000;
        
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
        // Lock Object for the SearchChannel list
        private Object channelsLock = new Object();

        // Timer that pings all the peers in the routing table
        System.Timers.Timer pingTable;
        System.Timers.Timer refreshTableTimer;
        // List to hold timers of pings
        Dictionary<System.Timers.Timer, int> pingTimers;
        private Object timersLock = new Object();
        // Event to update Form
        public event handlerUpdatedTable updateTable;
        public delegate void handlerUpdatedTable();

        // TODO: pass boot peer after retrieval of peers from servers
        public Peer()
        {
            // Initiating the timers
            refreshTableTimer = new System.Timers.Timer();
            refreshTableTimer.Elapsed += refreshTable;
            channels = new List<SearchChannel>();
            
            // List that holds the timers that waiting for ping response
            pingTimers = new Dictionary<System.Timers.Timer, int>();
            
            // Boot peer info
            bootPeer = new PeerInfo(0, "192.168.1.12", 8080);
        }

        // Timed event refreshinf the table
        void refreshTable(object sender, ElapsedEventArgs e)
        {
            refreshTableTimer.Stop();
            refreshTableTimer.Interval = REFRESH_TIME;
            searchTargetPeers();
            refreshTableTimer.Start();
        }

        // Used for initializing peer features requires guid, ip and port of the peer
        public void init(int guid, string ip, int port)
        {
            // Initializing myInfo
            myInfo = new PeerInfo(guid,ip,port);

            // Create UDP listen and add events
            listener = new UDPListener(port);
            subscribeToListener(listener);
            
            // Create Routing Table and adding boot peer
            routingTable = new RoutingTable(myInfo);
            routingTable.add(bootPeer);

            // Create EndPoint
            IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse(bootPeer.getIP()), bootPeer.getPORT());
            // Sending routing table request
            UDPResponder responder = new UDPResponder(remotePoint, port);
            responder.sendRequestRoutingTable();
        }

        public void startPingTimer()
        {
            pingTable = new System.Timers.Timer(PING_INTERVALS);
            pingTable.Elapsed += sendPingToTable;
            pingTable.Start();
        }

        /// <summary>
        /// SEND REQUESTS
        /// </summary>
        
        // Send ping to all the peers in the table
        public void sendPingToTable(object sender, EventArgs e)
        {
            List<PeerInfo> list = new List<PeerInfo>(routingTable.getPeers().Values);
            foreach (PeerInfo p in list)
            {
                sendPing(p);
            }
        }

        // Send ping to see if alive
        private void sendPing(PeerInfo pInfo)
        {
            IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse(pInfo.getIP()), pInfo.getPORT());
            UDPResponder responder = new UDPResponder(remotePoint, myInfo.getPORT());
            responder.sendPing();
            startTimeoutTimer(pInfo.getGUID);
        }

        private void startTimeoutTimer(int guid)
        {
            System.Timers.Timer tempTimer = new System.Timers.Timer(WAIT_TIME_FOR_PING_RESPONSE);
            tempTimer.Elapsed += removeOffline;
            pingTimers.Add(tempTimer, guid);
            tempTimer.Start();
        }

        // Starting refresh timer with specified interval. Used for forced refresh.
        public void startRefreshTableTimer(int interval)
        {
            if (refreshTableTimer.Enabled)
            {
                refreshTableTimer.Stop();
            }
            refreshTableTimer.Interval = interval;
            refreshTableTimer.Start();
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
                if (closest != null && myInfo.getGUID != closest.getGUID)
                {
                    //routingTable.remove(closest.getGUID);
                    SearchChannel channel = new SearchChannel(this, guid);
                    channel.onReceiveClosest(closest);
                    lock (channelsLock)
                    {
                        channels.Add(channel);
                    }
                }
            }
        }

        // Sending a leave request to all the peers in your table
        public void sendLeaveRequests() {
            List<PeerInfo> list = new  List<PeerInfo>(routingTable.getPeers().Values);
            foreach(PeerInfo p in list)
            {
                sendLeaveRequest(p);
            }
        }

        // Sends leave request to the pInfo peer
        private void sendLeaveRequest(PeerInfo pInfo)
        {
            listener.closeSocket();
            pingTable.Stop();
            refreshTableTimer.Stop();
            IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse(pInfo.getIP()), pInfo.getPORT());
            UDPResponder responder = new UDPResponder(remotePoint, myInfo.getPORT());
            responder.sendRequestLeave( myInfo.getGUID );
        }

        /// <summary>
        /// EVENT HANDLERS
        /// </summary>

        // If peer did not respond to ping this function will be called by the timer.
        void removeOffline(object sender, ElapsedEventArgs e)
        {
            lock (timersLock)
            {
                System.Timers.Timer temp = (System.Timers.Timer)sender;
                temp.Stop();
                int guid = pingTimers[temp];
                pingTimers.Remove(temp);
                temp.Dispose();

                routingTable.remove(guid);
                startRefreshTableTimer(5000);
                updateTable();
            }
        }

        // Subscribe to UDP listener events
        public void subscribeToListener(UDPListener l)
        {
            l.receiveRequestTable += new UDPListener.handlerRequestTable(handleRequestTable);
            l.receiveTable += new UDPListener.handlerTable(handleTable);
            l.receiveClosest += new UDPListener.handlerFindChannel(handleReceiveClosest);
            l.receiveRequestClosest += new UDPListener.handlerRequestClosest(handleRequestClosest);
            l.receiveRequestJoin += new UDPListener.handlerRequestJoin(handleJoinRequest);
            l.receiveRequestLeave += new UDPListener.handlerRequestLeave(handleLeaveRequest);
            l.receiveRequestPing += new UDPListener.handlerRequestPing(handleRequestPing);
            l.receiveGUID += new UDPListener.handlerGUID(handleGUID);
        }

        // Handling ping request
        public void handleRequestPing(IPEndPoint remotePoint)
        {
            UDPResponder responder = new UDPResponder(remotePoint, remotePoint.Port);
            responder.sendGUID(myInfo.getGUID);
        }

        // Handling GUID
        public void handleGUID(int guid)
        {
            lock (timersLock)
            {
                System.Timers.Timer tempTimer = pingTimers.FirstOrDefault(x => x.Value == guid).Key;
                if (tempTimer != null)
                {
                    tempTimer.Stop();
                    pingTimers.Remove(tempTimer);
                    tempTimer.Dispose();
                }
            }
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
            startPingTimer();
            startRefreshTableTimer(30000);
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
            startRefreshTableTimer(1000);
            //searchTargetPeers();
            updateTable();
        }

        // Handling receive routing table request
        public void handleReceiveClosest(int guid, PeerInfo currentClosest)
        {
            routingTable.cleanTable(myInfo.getGUID);
            lock(channelsLock){
                foreach (SearchChannel c in channels)
                {
                    if (guid == c.getTargetGUID())
                    {
                        c.onReceiveClosest(currentClosest);
                    }
                }
            }
        }

        // Handles request to add peer info
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

        /// <summary>
        /// GET AND SET
        /// </summary>
        
        // Getting routing table
        public RoutingTable getRoutingTable
        {
            get { return this.routingTable; }
        }

        // Getting peer GUID
        public int getGUID
        {
            get { return myInfo.getGUID; }
        }
    }
}
